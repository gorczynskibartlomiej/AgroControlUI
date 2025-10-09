using AgroControlUI.Models.CropProtectionProducts;
using AgroControlUI.Models.ReferenceData;

namespace AgroControlUI.Models.FieldWorks
{
    public class SprayingWorkCropProtectionProduct
    {
        public int SprayingWorkId { get; set; }
        public int CropProtectionProductId { get; set; }
        public decimal Quantity { get; set; }
        public int UnitId { get; set; }
        public required Unit Unit { get; set; }
        public required SprayingWork SprayingWork { get; set; }
        public required CropProtectionProduct CropProtectionProduct { get; set; }
    }
}
