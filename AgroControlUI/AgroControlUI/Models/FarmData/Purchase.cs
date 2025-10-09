using AgroControlUI.Models.CropProtectionProducts;
using AgroControlUI.Models.Fertilizers;
using AgroControlUI.Models.ReferenceData;

namespace AgroControlUI.Models.FarmData
{
    public abstract class Purchase
    {
        public int Id { get; set; }
        public int FarmId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal PricePerUnit { get; set; }
        public string? Description { get; set; }
        public required string SupplierName { get; set; }
        public int UnitId { get; set; }
        public decimal Quantity { get; set; }

        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }

        public required Farm Farm { get; set; }
        public required Unit Unit { get; set; }
    }
    public class PurchaseFertilizer : Purchase
    {
        public int FertilizerId { get; set; }
        public required Fertilizer Fertilizer { get; set; }
    }
    public class PurchaseCropProtectionProduct : Purchase
    {
        public int CropProtectionProductId { get; set; }
        public required CropProtectionProduct CropProtectionProduct { get; set; }
    }

    public class PurchaseFuel : Purchase
    {
        public int FuelId { get; set; }
        public required Fuel Fuel { get; set; }
    }
    public class PurchaseSeed : Purchase
    {
        public int SeedId { get; set; }
        public required Seed Seed { get; set; }
    }
}