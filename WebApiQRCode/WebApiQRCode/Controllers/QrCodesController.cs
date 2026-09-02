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
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateQrCode(
        int id, UpdateQrCodeRequest model)
    {
        var email = User.FindFirstValue(ClaimTypes.Email)
                    ?? User.FindFirstValue("email");

        if (string.IsNullOrEmpty(email))
            return Unauthorized();

        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
            return NotFound();

        var qrCode = await qrDbContext.QrCodes
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == user.Id);

        if (qrCode == null)
            return NotFound("QR Code не знайдено");

        if (!qrCode.IsActive)
            return BadRequest("Деактивований QR Code не можна редагувати");

        qrCode.Name = model.Name;
        qrCode.TargetUrl = model.TargetUrl;
        qrCode.UpdatedAt = DateTime.UtcNow;

        await qrDbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpPatch("{id:int}/deactivate")]
    public async Task<IActionResult> DeactivateQrCode(int id)
    {
        var email = User.FindFirstValue(ClaimTypes.Email)
                    ?? User.FindFirstValue("email");

        if (string.IsNullOrEmpty(email))
            return Unauthorized();

        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
            return NotFound();

        var qrCode = await qrDbContext.QrCodes
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == user.Id);

        if (qrCode == null)
            return NotFound("QR Code не знайдено");

        if (!qrCode.IsActive)
            return BadRequest("QR Code вже деактивований");

        qrCode.IsActive = false;
        qrCode.UpdatedAt = DateTime.UtcNow;

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