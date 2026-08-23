using WebApiQRCode.Data.Entities.Identity;

namespace WebApiQRCode.Interfaces;

public interface IJwtTokenService
{
    Task<string> CreateTokenAsync(UserEntity user);
}