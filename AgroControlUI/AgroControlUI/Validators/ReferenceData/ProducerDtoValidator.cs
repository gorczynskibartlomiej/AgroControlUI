using AgroControlUI.DTOs.ReferenceData;
using FluentValidation;

namespace AgroControlUI.Validators.ReferenceData
{
    public class ProducerDtoValidator : AbstractValidator<ProducerDto>
    {
        public ProducerDtoValidator()
        {
            RuleFor(agriculturalEquipmentType => agriculturalEquipmentType.Name)
               .NotEmpty().WithMessage("Nazwa producenta jest wymagana.")
               .MaximumLength(50).WithMessage("Nazwa producenta nie może przekraczać 50 znaków.")
               .MinimumLength(2).WithMessage("Nazwa producenta musi zawierać co najmniej 2 znaki.");
        }
    }
}
