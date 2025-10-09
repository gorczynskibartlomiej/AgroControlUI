using AgroControlUI.DTOs.CropProtectionProducts;
using FluentValidation;

namespace AgroControlUI.Validators.CropProtectionProducts
{
    public class CreateCropProtectionProductComponentValidator : AbstractValidator<CreateCropProtectionProductComponent>
    {
        public CreateCropProtectionProductComponentValidator()
        {
            RuleFor(x => x.ActiveIngredientId)
                .NotNull().WithMessage("Składnik aktywny nie może być pusty.")
                .GreaterThanOrEqualTo(1).WithMessage("Składnik aktywny musi być wybrany z listy.");

            RuleFor(x => x.Concentration)
                .NotNull().WithMessage("Ilość składnika nie może być pusta.")
                .GreaterThanOrEqualTo(0).WithMessage("Ilość składnika aktywnego musi być większa lub równa 0.");
        }
    }
}
