using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApiQRCode.Data;
using WebApiQRCode.Data.Entities.Identity;
using WebApiQRCode.Extensions;

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

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

const string reactCorsPolicy = "ReactClient";
builder.Services.AddCors(options =>
{
    options.AddPolicy(reactCorsPolicy, policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
