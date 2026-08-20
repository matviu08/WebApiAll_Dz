using Microsoft.AspNetCore.Identity;

namespace WebApiQRCode.Data.Entities.Identity;

public class RoleEntity : IdentityRole<int>
{
    public ICollection<UserRoleEntity>? UsersRoles { get; set; }
}