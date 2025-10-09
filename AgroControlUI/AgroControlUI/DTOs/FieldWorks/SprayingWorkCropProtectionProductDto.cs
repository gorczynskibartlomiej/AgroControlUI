using AgroControlUI.DTOs.CropProtectionProducts;
using AgroControlUI.DTOs.ReferenceData;

namespace AgroControlUI.DTOs.FieldWorks
{
    public class SprayingWorkCropProtectionProductDto
    {
        public decimal Quantity { get; set; }
        public required UnitDto Unit { get; set; }
        public required CropProtectionProductDto CropProtectionProduct { get; set; }
    }
}
