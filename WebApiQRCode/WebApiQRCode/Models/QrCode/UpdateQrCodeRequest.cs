namespace WebApiQRCode.Models.QrCode;

public class UpdateQrCodeRequest
{
    public string Name { get; set; } = null!;
    public string TargetUrl { get; set; } = null!;
}