using AgroControlUI.DTOs.CropProtectionProducts;
using FluentValidation;

namespace AgroControlUI.Validators.CropProtectionProducts
{
    public class CreateCropProtectionProductDtoValidator : AbstractValidator<CreateCropProtectionProductDto>
    {
        public CreateCropProtectionProductDtoValidator()
        {
            RuleFor(product => product.Name)
                .NotEmpty().WithMessage("Nazwa środka ochrony roślin jest wymagana.")
                .MinimumLength(2).WithMessage("Nazwa środka ochrony roślin musi zawierać co najmniej 2 znaki.")
                .MaximumLength(50).WithMessage("Nazwa środka ochrony roślin nie może przekraczać 50 znaków.");

            RuleFor(product => product.ProducerId)
                 .NotEmpty().WithMessage("Producent jest wymagany.");

            RuleFor(product => product.Description)
                .MaximumLength(500).WithMessage("Opis może mieć maksymalnie 500 znaków.")
                .When(product => !string.IsNullOrEmpty(product.Description));

            RuleFor(product => product.CropIds)
                .NotEmpty().WithMessage("Musisz wybrać co najmniej jedną roślinę.");

            RuleFor(product => product.CategoryIds)
                .NotEmpty().WithMessage("Musisz wybrać co najmniej jedną kategorię.");

            RuleFor(product => product.ActiveIngredients)
                .NotEmpty().WithMessage("Musisz dodać co najmniej jeden składnik aktywny.")
                .ForEach(ingredient =>
                {
                    // Zastosowanie walidatora dla każdego składnika aktywnego
                    ingredient.SetValidator(new CreateCropProtectionProductComponentValidator());
                });
        }
    }
}
