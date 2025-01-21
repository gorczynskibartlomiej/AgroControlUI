using AgroControlUI.Models.ReferenceData;

namespace AgroControlUI.Models.FarmData
{
    public class GrainSilo
    {
        public int Id { get; set; }
        //+pojemnosc calkowita do dodania pozniej
        public int CropId { get; set; }
        public int UnitId { get; set; }
        public int FarmId { get; set; }
        public decimal Quantity { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }


        public required Crop Crop { get; set; }
        public required Unit Unit { get; set; }
        public required Farm Farm { get; set; }
    }
}
