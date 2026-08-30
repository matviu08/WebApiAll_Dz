using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiQRCode.Data;
using WebApiQRCode.Data.Entities;
using WebApiQRCode.Data.Entities.Identity;
using WebApiQRCode.Models.QrCode;

namespace WebApiQRCode.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class QrCodesController(QrCodeDbContext qrDbContext,
    UserManager<UserEntity> userManager) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetQrCodes()
    {
        var email = User.FindFirstValue(ClaimTypes.Email)
                    ?? User.FindFirstValue("email");

        if (string.IsNullOrEmpty(email))
            return Unauthorized();

        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
            return NotFound();

        var qrCodes = await qrDbContext.QrCodes
            .Where(x => x.UserId == user.Id)
            .Select(x => new QrCodeItemModel
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                TargetUrl = x.TargetUrl,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt.ToString("dd.MM.yyyy"),
                ScanCount = x.ScanCount
            })
            .ToListAsync();

        return Ok(qrCodes);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateQrCode(
        CreateQrCodeRequest model)
    {
        var email = User.FindFirstValue(ClaimTypes.Email)
                    ?? User.FindFirstValue("email");

        if (string.IsNullOrEmpty(email))
            return Unauthorized();

        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
            return NotFound();

        var qrCode = new QrCodeEntity
        {
            Name = model.Name,
            TargetUrl = model.TargetUrl,
            Code = Guid.NewGuid().ToString("N"),
            UserId = user.Id,
        };

        qrDbContext.QrCodes.Add(qrCode);

        await qrDbContext.SaveChangesAsync();

        return Ok();
    }
    
    [HttpGet("view/{code}")]
    [AllowAnonymous]
    public async Task<IActionResult> RedirectToTarget(string code)
    {
        var qrCode = await qrDbContext.QrCodes
            .FirstOrDefaultAsync(x => x.Code == code);

        if (qrCode == null)
            return NotFound("QR Code не знайдено");

        if (!qrCode.IsActive)
            return BadRequest("QR Code деактивований");

        qrCode.ScanCount++;

        await qrDbContext.SaveChangesAsync();

        return Redirect(qrCode.TargetUrl);
    }
}