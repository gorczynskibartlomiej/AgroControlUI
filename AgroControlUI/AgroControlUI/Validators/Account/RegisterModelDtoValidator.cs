using AgroControlUI.DTOs.Account;
using FluentValidation;

namespace AgroControlUI.Validators.Account
{
    public class RegisterModelDtoValidator:AbstractValidator<RegisterModelDto>
    {
        public RegisterModelDtoValidator()
        {
            RuleFor(register => register.FirstName)
                .NotEmpty().WithMessage("Imię jest wymagane.")
                .MaximumLength(50).WithMessage("Imię nie może przekraczać 50 znaków.")
                .MinimumLength(2).WithMessage("Imię musi zawierać co najmniej 2 znaki.")
                .Matches("^[a-zA-ZąćęłńóśźżĄĆĘŁŃÓŚŹŻ ]*$").WithMessage("Imię może zawierać tylko litery.");

            RuleFor(register => register.LastName)
                .NotEmpty().WithMessage("Nazwisko jest wymagane.")
                .MaximumLength(50).WithMessage("Nazwisko nie może przekraczać 50 znaków.")
                .MinimumLength(2).WithMessage("Nazwisko musi zawierać co najmniej 2 znaki.")
                .Matches("^[a-zA-ZąćęłńóśźżĄĆĘŁŃÓŚŹŻ ]*$").WithMessage("Nazwisko może zawierać tylko litery.");

            RuleFor(register => register.Email)
                .NotEmpty().WithMessage("Adres email jest wymagany.")
                .EmailAddress().WithMessage("Proszę podać poprawny adres e-mail.");

            RuleFor(register => register.Password)
                .NotEmpty().WithMessage("Hasło jest wymagane.")
                .MinimumLength(8).WithMessage("Hasło musi zawierać co najmniej 8 znaków.")
                .Matches(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).*$")
                .WithMessage("Hasło musi zawierać co najmniej 8 znaków, jedną wielką literę, jedną małą literę oraz jedną cyfrę.");

            RuleFor(register => register.ReapeatedPassword)
             .NotEmpty().WithMessage("Powtórz hasło.")
            .Equal(x => x.Password).WithMessage("Hasła nie są takie same.");
        }
    }
}
