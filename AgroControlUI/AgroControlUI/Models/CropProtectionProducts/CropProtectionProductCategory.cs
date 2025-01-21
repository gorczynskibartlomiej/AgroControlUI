namespace AgroControlUI.Models.CropProtectionProducts
{
    public class CropProtectionProductCategory
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public ICollection<CropProtectionProductCategoryProduct> CropProtectionProductCategoryProducts { get; set; } = new List<CropProtectionProductCategoryProduct>();
    }
}
