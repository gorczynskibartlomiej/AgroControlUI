namespace AgroControlUI.Models.CropProtectionProducts
{
    public class CropProtectionProductCategoryProduct
    {
        public int CropProtectionProductId { get; set; }
        public int CropProtectionProductCategoryId { get; set; }

        public required CropProtectionProduct CropProtectionProduct { get; set; }
        public required CropProtectionProductCategory CropProtectionProductCategory { get; set; }
    }
}
