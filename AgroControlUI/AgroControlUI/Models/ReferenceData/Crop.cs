using AgroControlUI.Models.CropProtectionProducts;
using AgroControlUI.Models.FarmData;
using AgroControlUI.Models.FieldWorks;

namespace AgroControlUI.Models.ReferenceData
{
    public class Crop
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public ICollection<CropProtectionProductCrop> CropProtectionProductCrops { get; set; } = new List<CropProtectionProductCrop>();
        public ICollection<GrainSilo> GrainSilos { get; set; } = new List<GrainSilo>();
        public ICollection<Seed> Seeds { get; set; } = new List<Seed>();
        public ICollection<CropRotationPlanner> CropRotationPlanners { get; set; } = new List<CropRotationPlanner>();
        public ICollection<HarvestingWork> HarvestingWorks { get; set; } = new List<HarvestingWork>();
    }
}
