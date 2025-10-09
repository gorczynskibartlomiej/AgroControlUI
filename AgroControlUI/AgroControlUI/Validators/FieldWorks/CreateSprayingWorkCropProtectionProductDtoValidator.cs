using AgroControlUI.DTOs.FieldWorks;
using FluentValidation;

namespace AgroControlUI.Validators.FieldWorks
{
    public class CreateSprayingWorkCropProtectionProductDtoValidator:AbstractValidator<CreateSprayingWorkCropProtectionProductDto>
    {
        public CreateSprayingWorkCropProtectionProductDtoValidator()
        {
            RuleFor(x => x.Quantity)
               .GreaterThan(0).WithMessage("Ilość musi być większa niż 0.");

            RuleFor(x => x.UnitId)
                .GreaterThan(0).WithMessage("Jednostka miary jest wymagana.");

            RuleFor(x => x.CropProtectionProductId)
                .GreaterThan(0).WithMessage("Środek ochrony roślin jest wymagany.");
        }
    }
}
