using AgroControlUI.DTOs.Account;
using FluentValidation;

namespace AgroControlUI.Validators.Account
{
    public class ChangePasswordDtoValidator:AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Aktualne hasło jest wymagane.");

            RuleFor(changePass => changePass.NewPassword)
                .NotEmpty().WithMessage("Nowe hasło jest wymagane.")
                .MinimumLength(8).WithMessage("Hasło musi zawierać co najmniej 8 znaków.")
                .Matches(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).*$")
                .WithMessage("Nowe hasło musi zawierać co najmniej 8 znaków, jedną wielką literę, jedną małą literę oraz jedną cyfrę.");

            RuleFor(changePass => changePass.ConfirmPassword)
             .NotEmpty().WithMessage("Potwierdź nowe hasło.")
            .Equal(x => x.NewPassword).WithMessage("Hasła nie są takie same.");
        }
    }
}
