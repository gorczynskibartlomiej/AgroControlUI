using AgroControlUI.Constants;
using AgroControlUI.DTOs.Fertilizers;
using FluentValidation;

namespace AgroControlUI.Validators.Fertilizers
{
    public class FertilizerCategoryDtoValidator : AbstractValidator<FertilizerCategoryDto>
    {
        public FertilizerCategoryDtoValidator()
        {
            RuleFor(fertilizerCategory => fertilizerCategory.Name)
                .NotEmpty().WithMessage("Nazwa kategorii nawozu jest wymagana.")
                .MaximumLength(50).WithMessage($"Nazwa kategorii nawozu nie może przekraczać 50 znaków.")
                .MinimumLength(2).WithMessage("Nazwa kategorii nawozu musi zawierać co najmniej 2 znaki.");
        }
    }
}
