using AgroControlUI.DTOs.ReferenceData;
using FluentValidation;

namespace AgroControlUI.Validators.ReferenceData
{
    public class AgriculturalEquipmentTypeDtoValidator : AbstractValidator<AgriculturalEquipmentTypeDto>
    {
        public AgriculturalEquipmentTypeDtoValidator()
        {
            RuleFor(agriculturalEquipmentType => agriculturalEquipmentType.Name)
               .NotEmpty().WithMessage("Nazwa rodzaju maszyn jest wymagana.")
               .MaximumLength(50).WithMessage("Nazwa rodzaju maszyn nie może przekraczać 50 znaków.")
               .MinimumLength(2).WithMessage("Nazwa rodzaju maszyn musi zawierać co najmniej 2 znaki.");
        }
    }
}
