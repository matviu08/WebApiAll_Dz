using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApiQRCode.Constants;
using WebApiQRCode.Data;
using WebApiQRCode.Data.Entities.Identity;
using WebApiQRCode.Models.Seeder;

namespace WebApiQRCode.Extensions;

public static class DbSeeder{
    public static async Task SeedData(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        
        var context = scope.ServiceProvider.GetRequiredService<QrCodeDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
        
        context.Database.Migrate();

        if (!context.Roles.Any())
        {
            foreach (var roleName in Roles.ListRoles())
            {
                await roleManager.CreateAsync(new RoleEntity { Name = roleName });
            }
        }

        if (!context.Users.Any())
        {
            var curDir = Directory.GetCurrentDirectory();
            var jsonFile = Path.Combine(curDir, "Helpers", "JsonData", "Users.json");
            if (File.Exists(jsonFile))
            {
                var jsonData = await File.ReadAllTextAsync(jsonFile);

                try
                {
                    var users = JsonSerializer.Deserialize<List<SeederUserModel>>(jsonData);
                    foreach (var user in users)
                    {
                        var entity = new UserEntity
                        {
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            UserName = user.Email,
                            Image = user.Image,
                        };
                        var result = await userManager.CreateAsync(entity, user.Password);
                        if (result.Succeeded)
                        {
                            foreach (var role in user.Roles)
                            {
                                await userManager.AddToRoleAsync(entity, role);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Seed Users", ex.Message);
                }
            }
        }
    }
}