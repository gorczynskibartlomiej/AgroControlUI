using AgroControlUI.DTOs.FarmData;
using FluentValidation;
using System.Text.RegularExpressions;

namespace AgroControlUI.Validators.FarmData
{
    public class CreateAgriculturalEquipmentDtoValidator : AbstractValidator<CreateAgriculturalEquipmentDto>
    {
        public CreateAgriculturalEquipmentDtoValidator()
        {
            RuleFor(x => x.Name)
                 .NotEmpty().WithMessage("Nazwa maszyny jest wymagana.")
                 .MinimumLength(2).WithMessage("Nazwa maszyny musi zawierać co najmniej 2 znaki.")
                 .MaximumLength(50).WithMessage("Nazwa maszyny nie może przekraczać 50 znaków.");

            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("Marka jest wymagana.")
                .MaximumLength(50).WithMessage("Marka nie może przekraczać 50 znaków.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Opis nie może przekraczać 500 znaków.")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.AgriculturalEquipmentTypeId)
                .NotEmpty().WithMessage("Typ maszyny jest wymagany.");

            RuleFor(x => x.YearOfManufacture)
                .InclusiveBetween(1900, DateTime.Now.Year).WithMessage($"Rok produkcji musi być między 1900 a {DateTime.Now.Year}.");

            RuleFor(x => x.FuelCapacity)
                .GreaterThan(0).WithMessage("Pojemność zbiornika musi być większa niż 0.")
                .When(x => x.FuelCapacity.HasValue);

            RuleFor(x => x.EnginePower)
                .NotEmpty().WithMessage("Moc silnika jest wymagana.")
                .GreaterThan(0).WithMessage("Moc silnika musi być większa niż 0.")
                .When(x => x.EnginePower.HasValue);

            RuleFor(x => x.Weight)
                .NotEmpty().WithMessage("Szerokość jest wymagana.")
                .GreaterThanOrEqualTo(0).WithMessage("Waga musi być większa niż 0.")
                .When(x => x.Weight.HasValue);

            RuleFor(x => x.Width)
                .GreaterThanOrEqualTo(0).WithMessage("Szerokość musi być większa niż 0.")
                .PrecisionScale(18,2,false).WithMessage("Podaj maksymanlnie 2 liczby po ',' np. '5.55'.");

            RuleFor(x => x.WorkingWidth)
                .GreaterThanOrEqualTo(0).WithMessage("Szerokość robocza musi być większa niż 0.")
                .When(x => x.WorkingWidth.HasValue);

            RuleFor(x => x.Height)
                .GreaterThanOrEqualTo(0).WithMessage("Wysokość musi być większa niż 0.")
                .When(x => x.Height.HasValue);

            RuleFor(x => x.WorkingSpeed)
                .GreaterThanOrEqualTo(0).WithMessage("Prędkość robocza musi być większa niż 0.")
                .When(x => x.WorkingSpeed.HasValue);

            RuleFor(x => x.TransportSpeed)
                .GreaterThanOrEqualTo(0).WithMessage("Prędkość transportowa musi być większa niż 0.")
                .When(x => x.TransportSpeed.HasValue);

            RuleFor(x => x.LastServiceDate)
            .Must(date => !date.HasValue || date.Value.ToDateTime(new TimeOnly(0, 0)) <= DateTime.Now)
            .WithMessage("Ostatni serwis nie może być w przyszłości.");

            RuleFor(x => x.NextServiceDate)
                .GreaterThanOrEqualTo(x => x.LastServiceDate).WithMessage("Następny serwis musi być po ostatnim serwisie.")
                .When(x => x.LastServiceDate.HasValue && x.NextServiceDate.HasValue);
        }
    }
}
