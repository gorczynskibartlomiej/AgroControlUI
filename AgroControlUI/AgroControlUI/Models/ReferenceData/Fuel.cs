using AgroControlUI.Models.FarmData;

namespace AgroControlUI.Models.ReferenceData
{
    public class Fuel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? FuelSymbol { get; set; }

        public ICollection<AgriculturalEquipment> AgriculturalEquipment { get; set; } = new List<AgriculturalEquipment>();
        public ICollection<FarmFuel> FarmFuels { get; set; } = new List<FarmFuel>();
        public ICollection<PurchaseFuel> PurchaseFuels { get; set; } = new List<PurchaseFuel>();
    }
}
