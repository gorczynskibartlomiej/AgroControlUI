namespace AgroControlUI.Models.CropProtectionProducts
{
    public class ActiveIngredient
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public ICollection<CropProtectionProductComponent> CropProtectionProductComponents { get; set; } = new List<CropProtectionProductComponent>();
    }
}
