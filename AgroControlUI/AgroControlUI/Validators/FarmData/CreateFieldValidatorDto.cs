using AgroControlUI.DTOs.FarmData;
using FluentValidation;

namespace AgroControlUI.Validators.FarmData
{
    public class CreateFieldDtoValidator:AbstractValidator<CreateFieldDto>
    {
        public CreateFieldDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nazwa pola jest wymagana.")
                .MinimumLength(2).WithMessage("Nazwa pola musi zawierać co najmniej 2 znaki")
                .MaximumLength(100).WithMessage("Nazwa pola nie może przekraczać 100 znaków.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Opis nie może przekraczać 500 znaków.")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.Area)
                .Must(x => x != null && decimal.TryParse(x.ToString(), out _))
                    .WithMessage("Powierzchnia musi być liczbą!")
                .NotEmpty().WithMessage("Powierzchnia jest wymagana.")
                .GreaterThanOrEqualTo(0).WithMessage("Powierzchnia musi być większa niż 0.")
                .LessThanOrEqualTo(10000).WithMessage("Powierzchnia nie może być większa niż 10 000 ha.")
                .Must(x => x != 0).WithMessage("Powierzchnia nie może być równa 0.");

            RuleFor(x => x.SoilTypesIds)
                .NotEmpty().WithMessage("Pole musi mieć przypisany co najmniej jeden typ gleby.");
        }
    }
}
