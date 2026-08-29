namespace WebApiQRCode.Models.Account;

public class ProfileModel
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Image { get; set; }
    public IList<string> Roles { get; set; } = new List<string>();
}