namespace AgroControlUI.DTOs.CropProtectionProducts
{
    public class CropProtectionProductComponentDto
    {
        public int ComponentId { get; set; }
        public required string ActiveIngredientName { get; set; }
        public int Concentration { get; set; }
    }
}
