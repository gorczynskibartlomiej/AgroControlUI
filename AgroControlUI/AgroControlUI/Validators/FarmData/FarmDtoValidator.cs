using AgroControlUI.DTOs.FarmData;
using FluentValidation;

namespace AgroControlUI.Validators.FarmData
{
    public class FarmDtoValidator : AbstractValidator<FarmDto>
    {
        public FarmDtoValidator()
        {
            RuleFor(f => f.Name)
               .NotEmpty().WithMessage("Nazwa gospodarstwa jest wymagana.")
               .MinimumLength(2).WithMessage("Nazwa gospodarstwa musi zawierać co najmniej 2 znaki.")
               .MaximumLength(50).WithMessage("Nazwa gospodarstwa nie może przekraczać 50 znaków.");

            RuleFor(f => f.FarmNumber)
                .MaximumLength(50).WithMessage("Numer gospodarstwa nie może przekraczać 50 znaków.")
                .Matches(@"^[a-zA-Z0-9\-]*$").WithMessage("Numer gospodarstwa może zawierać tylko litery, cyfry i myślniki.")
                .When(f => !string.IsNullOrEmpty(f.FarmNumber));

            RuleFor(f => f.Description)
                .MaximumLength(500).WithMessage("Opis nie może przekraczać 500 znaków.")
                .When(f => !string.IsNullOrEmpty(f.Description));

            RuleFor(f => f.YearEstablished)
                .InclusiveBetween(1800, DateTime.Now.Year)
                .WithMessage($"Rok założenia gospodarstwa musi być między 1800 a {DateTime.Now.Year}.");

            RuleFor(f => f.Adress)
                .NotEmpty().WithMessage("Adres gospodarstwa jest wymagany.")
                .MaximumLength(50).WithMessage("Adres nie może przekraczać 50 znaków.");

            RuleFor(f => f.PostalCode)
                .NotEmpty().WithMessage("Kod pocztowy jest wymagany.")
                .Matches(@"^\d{2}-\d{3}$").WithMessage("Kod pocztowy musi być w formacie XX-XXX.");

            RuleFor(f => f.City)
                .NotEmpty().WithMessage("Miasto jest wymagane.")
                .MaximumLength(50).WithMessage("Nazwa miasta nie może przekraczać 50 znaków.");

        }
    }
}
