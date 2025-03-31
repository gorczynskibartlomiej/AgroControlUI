using AgroControlUI.DTOs.Fertilizers;
using FluentValidation;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AgroControlUI.Validators.Fertilizers
{
    public class CreateFertilizerComponentDtoValidator : AbstractValidator<CreateFertilizerComponentDto>
    {
        public CreateFertilizerComponentDtoValidator()
        {
            RuleFor(component => component.ChemicalElementId)
               .NotEmpty().WithMessage("Musisz wybrać składnik nawozu.");

            RuleFor(component => component.ElementPercentage)
    .NotNull().WithMessage("Składnik nawozu musi mieć przypisaną zawartość.")
    .Must(value =>
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        decimal elementPercentage;
        return decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out elementPercentage)
               && elementPercentage > 0 && elementPercentage <= 100;
    })
    .WithMessage("Składnik nawozu nie może przekraczać 100%.");
        }
    }
}
