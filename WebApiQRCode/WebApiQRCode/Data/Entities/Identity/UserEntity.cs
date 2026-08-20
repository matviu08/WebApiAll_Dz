using Microsoft.AspNetCore.Identity;

namespace WebApiQRCode.Data.Entities.Identity;

public class UserEntity : IdentityUser<int>
{
    public ICollection<UserRoleEntity>? UserRoles { get; set; }
}