using AgroControlUI.DTOs.CropProtectionProducts;
using FluentValidation;

namespace AgroControlUI.Validators.CropProtectionProducts
{
    public class ActiveIngredientDtoValidator : AbstractValidator<ActiveIngredientDto>
    {
        public ActiveIngredientDtoValidator()
        {
            RuleFor(activeIngredient => activeIngredient.Name)
               .NotEmpty().WithMessage("Nazwa substancji aktywnej jest wymagana.")
               .MaximumLength(50).WithMessage("Nazwa substancji aktywnej nie może przekraczać 50 znaków.")
               .MinimumLength(2).WithMessage("Nazwa substancji aktywnej musi zawierać co najmniej 2 znaki.");
        }
    }
}
