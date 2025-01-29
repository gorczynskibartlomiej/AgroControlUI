using AgroControlUI.DTOs.Fertilizers;
using FluentValidation;
using System.Xml.Linq;

namespace AgroControlUI.Validators.Fertilizers
{
    public class ChemicalElementDtoValidator : AbstractValidator<ChemicalElementDto>
    {
        public ChemicalElementDtoValidator()
        {
            RuleFor(chemicalElement => chemicalElement.Name)
               .NotEmpty().WithMessage("Nazwa składnika chemicznego jest wymagana.")
               .MaximumLength(50).WithMessage("Nazwa składnika chemicznego nie może przekraczać 50 znaków.")
               .MinimumLength(2).WithMessage("Nazwa składnika chemicznego musi zawierać co najmniej 2 znaki.");

            RuleFor(chemicalElement => chemicalElement.ChemicalSymbol)
               .NotEmpty().WithMessage("Nazwa składnika chemicznego jest wymagana.")
               .MaximumLength(2).WithMessage("Symbol chemiczny nie może przekraczać 2 znaków.");
        }
    }
}
