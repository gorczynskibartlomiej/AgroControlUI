using AgroControlUI.DTOs.ReferenceData;
using System.Net;

namespace AgroControlUI.DTOs.CropProtectionProducts
{
    public class CropProtectionProductDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string ProducerName { get; set; }

        public ICollection<CropDto> cropProtectionProductCrops = new List<CropDto>();
        public ICollection<CropProtectionProductComponentDto> cropProtectionProductComponents = new List<CropProtectionProductComponentDto>();
        public ICollection<CropProtectionProductCategoryDto> cropProtectionProductCategories = new List<CropProtectionProductCategoryDto>();
    }
}
