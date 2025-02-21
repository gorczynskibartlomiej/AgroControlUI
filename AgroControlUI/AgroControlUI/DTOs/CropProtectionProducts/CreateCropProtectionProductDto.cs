namespace AgroControlUI.DTOs.CropProtectionProducts
{
    public class CreateCropProtectionProductDto
    {
        public required string Name { get; set; }
        public int ProducerId { get; set; }
        public string? Description { get; set; }

        public List<int> CropIds { get; set; } = new List<int>();
        public List<CreateCropProtectionProductComponent> ActiveIngredients { get; set; } = new List<CreateCropProtectionProductComponent>();
        public List<int> CategoryIds { get; set; } = new List<int>();
    }
}
