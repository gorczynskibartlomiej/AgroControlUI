namespace AgroControlUI.Models.CropProtectionProducts
{
    public class CropProtectionProductComponent
    {
        public int CropProtectionProductId { get; set; }
        public int ActiveIngredientId { get; set; }
        public int Concentration { get; set; }

        public required CropProtectionProduct CropProtectionProduct { get; set; }
        public required ActiveIngredient ActiveIngredient { get; set; } 
    }
}
