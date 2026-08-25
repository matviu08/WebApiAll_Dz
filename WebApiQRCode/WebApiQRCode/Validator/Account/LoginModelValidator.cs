using FluentValidation;
using WebApiQRCode.Models.Account;

namespace WebApiQRCode.Validator.Account;

public class LoginModelValidator : AbstractValidator<LoginModel>
{
    public LoginModelValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email обов'язковий")
            .EmailAddress().WithMessage("Некоректний формат email")
            .MaximumLength(256).WithMessage("Email занадто довгий");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль обов'язковий")
            .MinimumLength(6).WithMessage("Пароль має містити щонайменше 6 символів")
            .MaximumLength(100).WithMessage("Пароль занадто довгий");
    }
}