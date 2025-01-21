using AgroControlUI.Models.FarmData;
using AgroControlUI.Models.FieldWorks;
using AgroControlUI.Models.ReferenceData;

namespace AgroControlUI.Models.CropProtectionProducts
{
    public class CropProtectionProduct
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int ProducerId { get; set; }
        public string? Description { get; set; }
        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }

        public ICollection<FarmCropProtectionProduct> FarmCropProtectionProducts { get; set; } = new List<FarmCropProtectionProduct>();
        public ICollection<PurchaseCropProtectionProduct> PurchaseCropProtectionProducts { get; set; } = new List<PurchaseCropProtectionProduct>();
        public ICollection<CropProtectionProductCrop> CropProtectionProductCrops { get; set; } = new List<CropProtectionProductCrop>();
        public ICollection<CropProtectionProductComponent> CropProtectionProductComponents { get; set; } = new List<CropProtectionProductComponent>();
        public ICollection<SprayingWorkCropProtectionProduct> SprayingWorkCropProtectionProducts { get; set; } = new List<SprayingWorkCropProtectionProduct>();
        public ICollection<CropProtectionProductCategoryProduct> CropProtectionProductCategoryProducts { get; set; } = new List<CropProtectionProductCategoryProduct>();
        public required Producer Producer { get; set; }

    }
}
