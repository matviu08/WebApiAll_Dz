namespace WebApiQRCode.Models.QrCode;

public class CreateQrCodeRequest
{
    public string Name { get; set; } = null!;
    public string TargetUrl { get; set; } = null!;
}