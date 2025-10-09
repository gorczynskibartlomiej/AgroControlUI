using AgroControlUI.DTOs.FieldWorks;
using FluentValidation;

namespace AgroControlUI.Validators.FieldWorks
{
    public class CreateSprayingWorkDtoValidator:AbstractValidator<CreateSprayingWorkDto>
    {
        public CreateSprayingWorkDtoValidator()
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

            RuleFor(x => x.SprayingWorkCropProtectionProducts)
                .NotEmpty().WithMessage("Musisz dodać przynajmniej jeden środek ochrony roślin.");

            RuleForEach(x => x.SprayingWorkCropProtectionProducts)
                .SetValidator(new CreateSprayingWorkCropProtectionProductDtoValidator());


            When(x => !x.EndTime.HasValue, () =>
            {
                RuleFor(x => x.StartTime)
                    .Must(start => start >= DateTime.Now)
                    .WithMessage("Data rozpoczęcia nie może być w przeszłości, jeśli nie podano daty zakończenia.");
            });

            When(x => x.EndTime.HasValue, () =>
            {
                RuleFor(x => x.EndTime)
                    .LessThanOrEqualTo(DateTime.Now)
                    .WithMessage("Data zakończenia nie może być w przyszłości.");

                RuleFor(x => x.EndTime)
                .Must((model, endTime) => endTime > model.StartTime)
                .WithMessage("Data zakończenia musi być po dacie rozpoczęcia.");
            });
        }
    }
}
