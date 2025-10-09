using AgroControlUI.DTOs.Fertilizers;
using AgroControlUI.DTOs.ReferenceData;

namespace AgroControlUI.DTOs.FieldWorks
{
    public class FertilizingWorkFertilizerDto
    {
        public decimal Quantity { get; set; }
        public required FertilizerDto Fertilizer { get; set; }
        public required UnitDto Unit { get; set; }
    }
}
