using AgroControlUI.DTOs.CropProtectionProducts;
using AgroControlUI.DTOs.ReferenceData;

namespace AgroControlUI.DTOs.FieldWorks
{
    public class CreateSprayingWorkCropProtectionProductDto
    {
        public decimal Quantity { get; set; }
        public int UnitId { get; set; }
        public int CropProtectionProductId { get; set; }
    }
}
