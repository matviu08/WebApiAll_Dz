using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApiQRCode.Data.Entities.Identity;

namespace WebApiQRCode.Data;

public class QrCodeDbContext : IdentityDbContext<UserEntity, RoleEntity, int>
{
    public QrCodeDbContext(DbContextOptions<QrCodeDbContext> options)
        : base(options)
    { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<UserRoleEntity>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRoleEntity>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UsersRoles)
            .HasForeignKey(ur => ur.RoleId);
    }
}