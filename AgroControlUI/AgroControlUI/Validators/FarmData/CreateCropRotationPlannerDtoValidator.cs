using AgroControlUI.DTOs.FarmData;
using FluentValidation;

namespace AgroControlUI.Validators.FarmData
{
    public class CreateCropRotationPlannerDtoValidator : AbstractValidator<CreateCropRotationPlannerDto>
    {
        public CreateCropRotationPlannerDtoValidator()
        {
            RuleFor(x => x.FieldId)
                .NotEmpty().WithMessage("Pole jest wymagane.");

            RuleFor(x => x.CropId)
                .NotEmpty().WithMessage("Uprawa jest wymagana.");

            RuleFor(x => x.Year)
                .InclusiveBetween(1900, DateTime.Now.Year+2)
                .WithMessage($"Rok uprawy musi być między 1900 a {DateTime.Now.Year+2}.");
        }
    }
}
