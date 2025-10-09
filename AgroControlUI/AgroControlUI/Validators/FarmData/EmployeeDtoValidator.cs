using AgroControlUI.DTOs.FarmData;
using FluentValidation;

namespace AgroControlUI.Validators.FarmData
{
    public class EmployeeDtoValidator : AbstractValidator<EmployeeDto>
    {
        public EmployeeDtoValidator()
        {
            RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Imię jest wymagane.")
                .MinimumLength(2).WithMessage("Imię musi zawierać co najmniej 2 znaki")
                .MaximumLength(100).WithMessage("Imię nie może przekraczać 100 znaków.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Nazwisko jest wymagane.")
                .MinimumLength(2).WithMessage("Nazwisko musi zawierać co najmniej 2 znaki")
                .MaximumLength(100).WithMessage("Nazwisko nie może przekraczać 100 znaków.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Numer telefonu jest wymagany.")
                .Matches(@"^\+?[0-9]{9,14}$").WithMessage("Numer telefonu musi zawierać od 9 do 15 cyfr i może zaczynać się od '+'.");
        }
    }
}
