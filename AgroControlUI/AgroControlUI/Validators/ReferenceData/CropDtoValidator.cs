using AgroControlUI.DTOs.ReferenceData;
using FluentValidation;

namespace AgroControlUI.Validators.ReferenceData
{
    public class CropDtoValidator : AbstractValidator<CropDto>
    {
        public CropDtoValidator()
        {
            RuleFor(crop => crop.Name)
               .NotEmpty().WithMessage("Nazwa uprawy jest wymagana.")
               .MaximumLength(50).WithMessage("Nazwa uprawy nie może przekraczać 50 znaków.")
               .MinimumLength(2).WithMessage("Nazwa uprawy musi zawierać co najmniej 2 znaki.");
        }
    }
}
