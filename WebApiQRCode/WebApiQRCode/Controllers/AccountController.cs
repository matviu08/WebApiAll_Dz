using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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
        IImageService imageService,
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
                if (model.ImageFile != null)
                {
                    user.Image = await imageService.SaveOptimizedImageAsync(model.ImageFile);
                }
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
        
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var email = User.FindFirstValue(ClaimTypes.Email)
                        ?? User.FindFirstValue("email");

            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound();

            var roles = await userManager.GetRolesAsync(user);

            var model = new ProfileModel
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Image = user.Image,
                Roles = roles
            };

            return Ok(model);
        }
    }
}
