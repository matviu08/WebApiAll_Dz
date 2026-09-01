using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApiQRCode.Data;
using WebApiQRCode.Data.Entities.Identity;
using WebApiQRCode.Extensions;
using WebApiQRCode.Filters;
using WebApiQRCode.Interfaces;
using WebApiQRCode.Services;
using WebApiQRCode.Validator.Account;

var builder = WebApplication.CreateBuilder(args);

string strConn = builder.Configuration
    .GetConnectionString("MyQRCodeConnection") ?? "";
builder.Services.AddDbContext<QrCodeDbContext>(opt =>
    opt.UseNpgsql(strConn));

builder.Services.AddIdentity<UserEntity, RoleEntity>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 1;
    })
    .AddEntityFrameworkStores<QrCodeDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IImageService, ImageOptimizationService>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(opt =>
{

    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

const string reactCorsPolicy = "ReactClient";
builder.Services.AddCors(options =>
{
    options.AddPolicy(reactCorsPolicy, policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173", 
                "https://qrcodemaneger.freehosting.dev"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddValidatorsFromAssemblyContaining<LoginModelValidator>();

builder.Services.Configure<MvcOptions>(options =>
{
    options.Filters.Add<ValidationFilter>();
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = long.MaxValue;
    options.MultipartHeadersLengthLimit = int.MaxValue;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }
await app.SeedData();

app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

app.UseCors(reactCorsPolicy);

try
{
    var myImage = builder.Configuration.GetRequiredSection("ImagesDir").Get<string>() ?? "myimages";
    string path = Path.Combine(Directory.GetCurrentDirectory(), myImage);
    Directory.CreateDirectory(path); //автоматично стоврить images

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(path),
        RequestPath = $"/{myImage}"
    });
}
catch (Exception ex)
{
    Console.WriteLine("Помилка запуску" + ex.Message);
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
