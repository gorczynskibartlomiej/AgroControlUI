using AgroControlUI.Models.FarmData;
using AgroControlUI.Models.FieldWorks;
using NetTopologySuite.Geometries;

namespace AgroControlUI.DTOs.FarmData
{
    public class FieldDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Area { get; set; }
        public ICollection<string> SoilTypes { get; set; } = new List<string>();
    }
}
