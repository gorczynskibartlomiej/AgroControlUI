using AgroControlUI.DTOs.ReferenceData;
using NetTopologySuite.Geometries;

namespace AgroControlUI.DTOs.FarmData
{
    public class FieldDetailsDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? FieldBorder { get; set; }
        public decimal Area { get; set; }
        public ICollection<SoilTypeDto> SoilTypes { get; set; } = new List<SoilTypeDto>();
        public DateTime CreatedOn { get; set; }
        public required string CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public required string UpdatedBy { get; set; }
    }
}
