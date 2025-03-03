using NetTopologySuite.Geometries;

namespace AgroControlUI.DTOs.FarmData
{
    public class CreateFieldDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? FieldBorder { get; set; }
        public decimal? Area { get; set; }
        public ICollection<int>? SoilTypesIds { get; set; } = new List<int>();
    }
}
