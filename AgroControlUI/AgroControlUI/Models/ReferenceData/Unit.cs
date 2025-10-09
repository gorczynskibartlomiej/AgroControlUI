using AgroControlUI.Models.FarmData;
using AgroControlUI.Models.Fertilizers;
using AgroControlUI.Models.FieldWorks;

namespace AgroControlUI.Models.ReferenceData
{
    public class Unit
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public ICollection<GrainSilo> GrainSilos { get; set; } = new List<GrainSilo>();
        public ICollection<FarmInventory> FarmInventories { get; set; } = new List<FarmInventory>();
        public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
        public ICollection<FertilizingWorkFertilizer> FertilizingWorkFertilizers { get; set; } = new List<FertilizingWorkFertilizer>();
        public ICollection<SprayingWorkCropProtectionProduct> SprayingWorkCropProtectionProducts { get; set; } = new List<SprayingWorkCropProtectionProduct>();
        public ICollection<HarvestingWork> HarvestingWorks { get; set; } = new List<HarvestingWork>();
    }
}
