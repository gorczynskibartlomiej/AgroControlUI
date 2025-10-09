using AgroControlUI.DTOs.FieldWorks;
using FluentValidation;

namespace AgroControlUI.Validators.FieldWorks
{
    public class CreateFertilizingWorkFertilizerDtoValidator:AbstractValidator<CreateFertilizingWorkFertilizerDto>
    {
        public CreateFertilizingWorkFertilizerDtoValidator()
        {
            RuleFor(x => x.Quantity)
               .GreaterThan(0).WithMessage("Ilość nawozu musi być większa niż 0.");

            RuleFor(x => x.FertilizerId)
                .GreaterThan(0).WithMessage("Identyfikator nawozu jest wymagany.");

            RuleFor(x => x.UnitId)
                .GreaterThan(0).WithMessage("Identyfikator jednostki miary jest wymagany.");
        }
    }
}
