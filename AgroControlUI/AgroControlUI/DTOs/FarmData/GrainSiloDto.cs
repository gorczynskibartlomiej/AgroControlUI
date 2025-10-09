using AgroControlUI.Models.FarmData;
using AgroControlUI.Models.ReferenceData;

namespace AgroControlUI.DTOs.FarmData
{
    public class GrainSiloDto
    {
        public int Id { get; set; }
        public required string CropName { get; set; }
        public required string Unit { get; set; }
        public decimal Quantity { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
    }
}
