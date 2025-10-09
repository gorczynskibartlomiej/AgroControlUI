using AgroControlUI.DTOs.FarmData;

namespace AgroControlUI.DTOs.FieldWorks
{
    public class FieldWorkAgriculturalEquipmentDto
    {
        public decimal? FuelUsed { get; set; }
        public required AgriculturalEquipmentDto AgriculturalEquipment { get; set; }
    }
}
