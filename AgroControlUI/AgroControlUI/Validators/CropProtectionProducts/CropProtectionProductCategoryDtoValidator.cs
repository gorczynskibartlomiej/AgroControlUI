using AgroControlUI.DTOs.CropProtectionProducts;
using FluentValidation;

namespace AgroControlUI.Validators.CropProtectionProducts
{
    public class CropProtectionProductCategoryDtoValidator : AbstractValidator<CropProtectionProductCategoryDto>
    {
        public CropProtectionProductCategoryDtoValidator()
        {
            RuleFor(cropProtectionProductCategory => cropProtectionProductCategory.Name)
               .NotEmpty().WithMessage("Nazwa kategorii środka ochrony roślin jest wymagana.")
               .MaximumLength(50).WithMessage("Nazwa kategorii środka ochrony roślin nie może przekraczać 50 znaków.")
               .MinimumLength(2).WithMessage("Nazwa kategorii środka ochrony roślin musi zawierać co najmniej 2 znaki.");
        }
    }
}
