using AgroControlUI.Models.Fertilizers;
using AgroControlUI.Models.ReferenceData;

namespace AgroControlUI.Models.FieldWorks
{
    public class FertilizingWorkFertilizer
    {
        public int FertilizingWorkId { get; set; }
        public int FertilizerId { get; set; }
        public int UnitId { get; set; }
        public decimal Quantity { get; set; }
        public required FertilizingWork FertilizingWork { get; set; }
        public required Fertilizer Fertilizer { get; set; }
        public required Unit Unit { get; set; }
    }
}
