using AgroControlUI.Models.CropProtectionProducts;
using AgroControlUI.Models.FarmData;
using AgroControlUI.Models.Fertilizers;
using AgroControlUI.Models.ReferenceData;

namespace AgroControlUI.DTOs.FarmData
{
    public class FarmInventoryDto
    {
        public int Id { get; set; }
        public int FarmId { get; set; }
        public decimal Quantity { get; set; }
        public decimal AverageUnitPrice { get; set; }
        public int UnitId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }

        public required Farm Farm { get; set; }
        public required Unit Unit { get; set; }
    }
    public class FarmCropProtectionProduct : FarmInventory
    {
        public int CropProtectionProductId { get; set; }
        public required CropProtectionProduct CropProtectionProduct { get; set; }
    }
    public class FarmFertilizer : FarmInventory
    {
        public int FertilizerId { get; set; }
        public required Fertilizer Fertilizer { get; set; }
    }
    public class FarmSeed : FarmInventory
    {
        public int SeedId { get; set; }
        //public decimal SeedWeight { get; set; }
        //public decimal GerminationRate { get; set; }
        //public decimal Purity { get; set; }
        public required Seed Seed { get; set; }
    }
    public class FarmFuel : FarmInventory
    {
        public int FuelId { get; set; }
        public required Fuel Fuel { get; set; }
    }
    
}
