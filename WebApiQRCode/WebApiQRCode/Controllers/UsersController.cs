using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiQRCode.Data;
using WebApiQRCode.Models.Users;

namespace WebApiQRCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(QrCodeDbContext qrDbContext) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await qrDbContext.Users
                .Select(x => new UserItemModel
                {
                    Id = x.Id,
                    FullName = $"{x.LastName} {x.FirstName}",
                    Email = x.Email,
                    Image = x.Image,
                }).ToListAsync();
            return Ok();
        }
    }
}
