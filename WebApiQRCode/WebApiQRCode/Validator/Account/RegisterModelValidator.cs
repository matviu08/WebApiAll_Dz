using FluentValidation;
using WebApiQRCode.Models.Account;

namespace WebApiQRCode.Validator.Account;

public class RegisterModelValidator : AbstractValidator<RegisterModel>
{
    private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
    private const long MaxImageSizeBytes = 5 * 1024 * 1024; // 5 MB

    public RegisterModelValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ім'я обов'язкове")
            .MaximumLength(50).WithMessage("Ім'я занадто довге");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Прізвище обов'язкове")
            .MaximumLength(50).WithMessage("Прізвище занадто довге");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email обов'язковий")
            .EmailAddress().WithMessage("Некоректний формат email")
            .MaximumLength(256).WithMessage("Email занадто довгий");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль обов'язковий")
            .MinimumLength(8).WithMessage("Мінімум 8 символів")
            .Matches("[A-Z]").WithMessage("Має містити хоча б одну велику літеру")
            .Matches("[a-z]").WithMessage("Має містити хоча б одну малу літеру")
            .Matches("[0-9]").WithMessage("Має містити хоча б одну цифру")
            .Matches("[^a-zA-Z0-9]").WithMessage("Має містити хоча б один спецсимвол");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Підтвердження пароля обов'язкове")
            .Equal(x => x.Password).WithMessage("Паролі не збігаються");

        RuleFor(x => x.ImageFile)
            .Must(HaveValidSize).WithMessage($"Розмір файлу не має перевищувати {MaxImageSizeBytes / 1024 / 1024} МБ")
            .Must(HaveValidExtension).WithMessage($"Дозволені формати: {string.Join(", ", AllowedImageExtensions)}")
            .When(x => x.ImageFile is not null);
    }

    private static bool HaveValidSize(IFormFile? file)
        => file is null || file.Length <= MaxImageSizeBytes;

    private static bool HaveValidExtension(IFormFile? file)
    {
        if (file is null) return true;
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return AllowedImageExtensions.Contains(extension);
    }
}
 