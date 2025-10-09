

namespace AgroControlUI.DTOs.FarmData
{
    public class AgriculturalEquipmentDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Brand { get; set; }
        public required string AgriculturalEquipmentTypeName { get; set; }
        public bool IsActive { get; set; }
    }
}
