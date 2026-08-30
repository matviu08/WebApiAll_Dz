using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiQRCode.Data.Entities.Identity;

namespace WebApiQRCode.Data.Entities;

public class QrCodeEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public UserEntity User { get; set; } = null!;
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;
    [Required]
    [StringLength(100)]
    public string Code { get; set; } = null!;
    [Required]
    [StringLength(2048)]
    public string TargetUrl { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public int ScanCount { get; set; }
}