using FluentValidation;
using WebApiQRCode.Models.QrCode;

namespace WebApiQRCode.Validator.QrCode;

public class CreateQrCodeRequestValidator : AbstractValidator<CreateQrCodeRequest>
{
    public CreateQrCodeRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Назва QR-коду обов'язкова")
            .MaximumLength(100).WithMessage("Назва QR-коду занадто довга");

        RuleFor(x => x.TargetUrl)
            .NotEmpty().WithMessage("URL обов'язковий")
            .MaximumLength(2048).WithMessage("URL занадто довгий")
            .Must(BeValidUrl).WithMessage("Некоректний формат URL");
    }

    private static bool BeValidUrl(string? url)
    {
        return Uri.TryCreate(
                   url,
                   UriKind.Absolute,
                   out var result)
               && (result.Scheme == Uri.UriSchemeHttp
                   || result.Scheme == Uri.UriSchemeHttps);
    }
}