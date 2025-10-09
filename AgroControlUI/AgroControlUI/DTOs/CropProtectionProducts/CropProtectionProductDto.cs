using AgroControlUI.DTOs.ReferenceData;
using System.Net;

namespace AgroControlUI.DTOs.CropProtectionProducts
{
    public class CropProtectionProductDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required ProducerDto Producer { get; set; }
        public string? Description { get; set; }

        public ICollection<CropDto> cropProtectionProductCrops { get; set; } = new List<CropDto>();
        public ICollection<CropProtectionProductComponentDto> cropProtectionProductComponents { get; set; } = new List<CropProtectionProductComponentDto>();
        public ICollection<CropProtectionProductCategoryDto> cropProtectionProductCategories { get; set; } = new List<CropProtectionProductCategoryDto>();
    }
}
