using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApiQRCode.Constants;
using WebApiQRCode.Data.Entities.Identity;
using WebApiQRCode.Interfaces;
using WebApiQRCode.Models.Account;

namespace WebApiQRCode.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController(IJwtTokenService jwtTokenService,
        UserManager<UserEntity> userManager) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user!=null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = await jwtTokenService.CreateTokenAsync(user);
                return Ok(new { Token = token });
            }
            return Unauthorized("Invalid login attempt");
        }
        
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterModel model)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                    throw new Exception("Дана пошта уже зареєстрована");

                user = new UserEntity
                {
                    Email = model.Email,
                    UserName = model.Email,
                    LastName = model.LastName,
                    FirstName = model.FirstName
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if(!result.Succeeded)
                {
                    var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                    throw new Exception(errors);
                }
                await userManager.AddToRoleAsync(user, Roles.User);

                var token = await jwtTokenService.CreateTokenAsync(user);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
