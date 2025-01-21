using AgroControlUI.Models.ReferenceData;

namespace AgroControlUI.Models.CropProtectionProducts
{
    public class CropProtectionProductCrop
    {
        public int CropProtectionProductId { get; set; }
        public int CropId { get; set; }

        public required CropProtectionProduct CropProtectionProduct { get; set; }
        public required Crop Crop { get; set; }
    }
}
