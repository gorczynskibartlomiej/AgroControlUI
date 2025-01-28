using AgroControlUI.Models.Fertilizers;
using AgroControlUI.Models.FieldWorks;
using AgroControlUI.Models.ReferenceData;

namespace AgroControlUI.DTOs.Fertilizers
{
    public class FertilizerDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string ProducerName { get; set; }
        public required string FertilizerCategoryName { get; set; }
        public ICollection<FertilizerComponentDto> fertilizerComponents { get; set; } = new List<FertilizerComponentDto>();
 
    }
}
