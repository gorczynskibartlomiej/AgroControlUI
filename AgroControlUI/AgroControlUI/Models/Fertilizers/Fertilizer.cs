using AgroControlUI.Models.FarmData;
using AgroControlUI.Models.FieldWorks;
using AgroControlUI.Models.ReferenceData;

namespace AgroControlUI.Models.Fertilizers
{
    public class Fertilizer
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int ProducerId { get; set; }
        public int FertilizerCategoryId { get; set; }

        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }

        public required Producer Producer { get; set; }
        public required FertilizerCategory FertilizerCategory { get; set; }
        public ICollection<FertilizerComponent> FertilizerComponents { get; set; } = new List<FertilizerComponent>();
        public ICollection<FarmFertilizer> FarmFertilizers { get; set; } = new List<FarmFertilizer>();
        public ICollection<PurchaseFertilizer> PurchaseFertilizers { get; set; } = new List<PurchaseFertilizer>();
        public ICollection<FertilizingWorkFertilizer> FertilizingWorkFertilizers { get; set; } = new List<FertilizingWorkFertilizer>();
    }
}
