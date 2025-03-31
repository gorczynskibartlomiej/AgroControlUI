using AgroControlUI.DTOs.UserManagement;
using FluentValidation;

namespace AgroControlUI.Validators.UserManagement
{
    public class InviteUserDtoValidator : AbstractValidator<InviteUserDto>
    {
        public InviteUserDtoValidator()
        {
            RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Adres email jest wymagany.")
            .EmailAddress().WithMessage("Proszę podać poprawny adres email.");


            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("Rola jest wymagana.");
        }
    }
}
