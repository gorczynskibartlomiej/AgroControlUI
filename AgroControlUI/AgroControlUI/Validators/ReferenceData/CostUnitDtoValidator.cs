using AgroControlUI.DTOs.ReferenceData;
using FluentValidation;

namespace AgroControlUI.Validators.ReferenceData
{
    public class CostUnitDtoValidator : AbstractValidator<CostUnitDto>
    {
        public CostUnitDtoValidator()
        {
            RuleFor(costUnit => costUnit.Name)
               .NotEmpty().WithMessage("Nazwa jednostki kosztów jest wymagana.")
               .MaximumLength(50).WithMessage("Nazwa jednostki kosztów nie może przekraczać 50 znaków.")
               .MinimumLength(2).WithMessage("Nazwa jednostki kosztów musi zawierać co najmniej 2 znaki.");
        }
    }
}
