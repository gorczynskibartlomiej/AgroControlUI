using AgroControlUI.DTOs.ReferenceData;
using FluentValidation;

namespace AgroControlUI.Validators.ReferenceData
{
    public class FuelDtoValidator : AbstractValidator<FuelDto>
    {
        public FuelDtoValidator()
        {
            RuleFor(fuel => fuel.Name)
               .NotEmpty().WithMessage("Nazwa rodzaju paliwa jest wymagana.")
               .MaximumLength(50).WithMessage("Nazwa rodzaju paliwa nie może przekraczać 50 znaków.")
               .MinimumLength(2).WithMessage("Nazwa rodzaju paliwa musi zawierać co najmniej 2 znaki.");

            RuleFor(fuel => fuel.FuelSymbol)
                .NotEmpty().WithMessage("Symbol paliwa jest wymagany.")
                .MaximumLength(10).WithMessage("Symbol paliwa nie może przekraczać 10 znaków");
        }
    }
}
