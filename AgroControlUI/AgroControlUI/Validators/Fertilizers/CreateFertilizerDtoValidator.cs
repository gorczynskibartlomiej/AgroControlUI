using AgroControlUI.DTOs.Fertilizers;
using FluentValidation;
using System.Globalization;

namespace AgroControlUI.Validators.Fertilizers
{
    public class CreateFertilizerDtoValidator : AbstractValidator<CreateFertilizerDto>
    {
        public CreateFertilizerDtoValidator()
        {
            RuleFor(fertilizer => fertilizer.Name)
                .NotEmpty().WithMessage("Nazwa nawozu jest wymagana.")
                .MinimumLength(2).WithMessage("Nazwa nawozu musi zawierać co najmniej 2 znaki.")
                .MaximumLength(50).WithMessage("Nazwa nawozu nie może przekroczyć 50 znaków.");

            RuleFor(fertilizer => fertilizer.ProducerId)
                .NotEmpty().WithMessage("Producent jest wymagany.");


            RuleFor(fertilizer => fertilizer.CategoryId)
                .NotEmpty().WithMessage("Kategoria nawozu jest wymagana.");

            RuleFor(fertilizer => fertilizer.Description)
                .MaximumLength(20000).WithMessage("Opis może mieć maksymalnie 20000 znaków.")
                .When(fertilizer => !string.IsNullOrEmpty(fertilizer.Description));

            RuleFor(fertilizer => fertilizer.FertilizerComponents)
             .Must(components => components.Sum(c =>
             {
                 decimal elementPercentage;
                 if (decimal.TryParse(c.ElementPercentage, NumberStyles.Any, CultureInfo.InvariantCulture, out elementPercentage))
                 {
                     return elementPercentage;
                 }
                 else
                 {
                     return 0;
                 }
             }) <= 100)
             .WithMessage("Suma zawartości wszystkich składników nawozu nie może przekraczać 100.")
             .NotEmpty().WithMessage("Musisz dodać co najmniej jeden składnik nawozu.")
             .ForEach(component =>
             {
                 component.SetValidator(new CreateFertilizerComponentDtoValidator());
             });
                }
    }
}
