using AgroControlUI.Models.FarmData;
using AgroControlUI.Models.FieldWorks;
using NetTopologySuite.Geometries;

namespace AgroControlUI.DTOs.FarmData
{
    public class FieldDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public Polygon? FieldBorder { get; set; }
        public decimal Area { get; set; }
        public string? SoilType { get; set; }

        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
    }
}
