using AgroControlUI.DTOs.Account;
using FluentValidation;

namespace AgroControlUI.Validators.Account
{
    public class LoginModelDtoValidator:AbstractValidator<LoginModelDto>
    {
        public LoginModelDtoValidator()
        {
            RuleFor(login => login.Email)
                .NotEmpty().WithMessage("Adres email jest wymagany.")
                .EmailAddress().WithMessage("Proszę podać poprawny adres e-mail.");

            RuleFor(login => login.Password)
                .NotEmpty().WithMessage("Hasło jest wymagane.");
        }
    }
}
