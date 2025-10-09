using AgroControlUI.DTOs.Fertilizers;
using AgroControlUI.DTOs.ReferenceData;

namespace AgroControlUI.DTOs.FieldWorks
{
    public class CreateFertilizingWorkFertilizerDto
    {
        public decimal Quantity { get; set; }
        public int FertilizerId { get; set; }
        public int UnitId { get; set; }
    }
}
