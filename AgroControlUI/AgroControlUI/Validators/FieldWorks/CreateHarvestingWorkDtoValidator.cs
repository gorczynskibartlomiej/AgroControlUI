using AgroControlUI.DTOs.FieldWorks;
using FluentValidation;

namespace AgroControlUI.Validators.FieldWorks
{
    public class CreateHarvestingWorkDtoValidator:AbstractValidator<CreateHarvestingWorkDto>
    {
        public CreateHarvestingWorkDtoValidator()
        {
            RuleFor(x => x.FieldId)
                 .NotEmpty().WithMessage("Pole jest wymagane.");

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Data rozpoczęcia jest wymagana.");

            RuleFor(x => x.EmployeeId)
               .Must((x, employeeId) => employeeId.HasValue || x.AgroControlUserId.HasValue)
               .WithMessage("Musisz przypisać pracownika lub użytkownika systemowego.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Opis nie może przekraczać 500 znaków.")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.FieldWorkAgriculturalEquipmentIds)
                .NotEmpty().WithMessage("Musisz wybrać przynajmniej jedną maszynę.");

            RuleFor(x => x.TotalYield)
                .NotEmpty().WithMessage("Plon całkowity jest wymagany.")
                .GreaterThan(0).WithMessage("Plon całkowity musi być większy niż 0.")
                .PrecisionScale(18, 2, false).WithMessage("Podaj maksymanlnie 2 liczby po ',' np. '5.55'."); ;

            RuleFor(x => x.Moisture)
                .InclusiveBetween(0, 100).WithMessage("Wilgotność musi być w zakresie od 0 do 100%.")
                .When(x => x.Moisture.HasValue)
                .PrecisionScale(18, 2, false).WithMessage("Podaj maksymanlnie 2 liczby po ',' np. '5.55'.")
                .When(x => x.Moisture.HasValue);

            RuleFor(x => x.CropId)
                .NotEmpty().WithMessage("Uprawa jest wymagana.");


            When(x => !x.EndTime.HasValue, () =>
            {
                RuleFor(x => x.StartTime)
                    .Must(start => start >= DateTime.Now)
                    .WithMessage("Data rozpoczęcia nie może być w przeszłości, jeśli nie podano daty zakończenia.");
            });

            When(x => x.EndTime.HasValue, () =>
            {
                RuleFor(x => x.EndTime.Value)
                    .LessThanOrEqualTo(DateTime.Now)
                    .WithMessage("Data zakończenia nie może być w przyszłości.");

                RuleFor(x => x.EndTime)
                .Must((model, endTime) => endTime > model.StartTime)
                .WithMessage("Data zakończenia musi być po dacie rozpoczęcia.");
            });
        }
    }
}
