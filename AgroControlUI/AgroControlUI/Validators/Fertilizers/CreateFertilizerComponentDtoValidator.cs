using AgroControlUI.DTOs.Fertilizers;
using FluentValidation;
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
        decimal elementPercentage;
        // Sprawdzamy, czy można przekonwertować string na decimal
        return decimal.TryParse(value, out elementPercentage) && elementPercentage <= 100;
    })
    .WithMessage("Składnik nawozu nie może przekraczać 100.");
        }
    }
}
