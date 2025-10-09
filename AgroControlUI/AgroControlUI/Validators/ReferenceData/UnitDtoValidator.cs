using AgroControlUI.DTOs.ReferenceData;
using FluentValidation;

namespace AgroControlUI.Validators.ReferenceData
{
    public class UnitDtoValidator : AbstractValidator<UnitDto>
    {
        public UnitDtoValidator()
        {
            RuleFor(agriculturalEquipmentType => agriculturalEquipmentType.Name)
               .NotEmpty().WithMessage("Nazwa jednostki jest wymagana.")
               .MaximumLength(50).WithMessage("Nazwa jednostki nie może przekraczać 50 znaków.");
        }
    }
}
