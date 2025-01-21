using AgroControlUI.Models.FarmData;

namespace AgroControlUI.Models.ReferenceData
{
    public class AgriculturalEquipmentType
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public ICollection<AgriculturalEquipment> AgriculturalEquipment { get; set; } = new List<AgriculturalEquipment>();
    }
}
