using AgroControlUI.Models.FieldWorks;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace AgroControlUI.Models.FarmData
{
    public class Field
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int FarmId { get; set; }
        public Polygon? FieldBorder { get; set; }
        public decimal Area { get; set; }
        public string? SoilType { get; set; }

        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }

        public required Farm Farm { get; set; }

        public ICollection<CropRotationPlanner> CropRotationPlanners { get; set; } = new List<CropRotationPlanner>();
        public ICollection<FieldWork> FieldWorks { get; set; } = new List<FieldWork>();
    }
}
