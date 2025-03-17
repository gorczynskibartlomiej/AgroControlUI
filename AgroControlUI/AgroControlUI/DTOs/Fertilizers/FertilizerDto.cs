using AgroControlUI.DTOs.ReferenceData;
using AgroControlUI.Models.Fertilizers;
using AgroControlUI.Models.FieldWorks;
using AgroControlUI.Models.ReferenceData;

namespace AgroControlUI.DTOs.Fertilizers
{
    public class FertilizerDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required ProducerDto Producer { get; set; }
        public required FertilizerCategoryDto FertilizerCategory { get; set; }
        public string? Description { get; set; }
        public ICollection<FertilizerComponentDto> FertilizerComponents { get; set; } = new List<FertilizerComponentDto>();
 
    }
}
