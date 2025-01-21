using AgroControlUI.Models.ReferenceData;

namespace AgroControlUI.DTOs.ReferenceData
{
    public class SeedDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Variety { get; set; }
        public required string CropName { get; set; }
        public required string ProducerName { get; set; }
        public string? Description { get; set; }
    }
}
