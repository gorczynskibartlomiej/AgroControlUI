using AgroControlUI.DTOs.FarmData;
using AgroControlUI.DTOs.ReferenceData;


namespace AgroControlUI.DTOs.FieldWorks
{
    public class FieldWorkDto
    {
        public int Id { get; set; }
        public int FieldId { get; set; }
        public bool IsPlanned { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? EmployeeId { get; set; }
        public int? AgroControlUserId { get; set; }
        public string? Description { get; set; }

        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }

        public ICollection<FieldWorkAgriculturalEquipmentDto> FieldWorkAgriculturalEquipment { get; set; } = new List<FieldWorkAgriculturalEquipmentDto>();
        public required FieldDto Field { get; set; }
        public string? EmployeeName { get; set; }
        public string? AgroControlUserName  { get; set; }
    }
    public class FertilizingWorkDto : FieldWorkDto
    {
        public ICollection<FertilizingWorkFertilizerDto> FertilizingWorkFertilizers { get; set; } = new List<FertilizingWorkFertilizerDto>();
    }
    public class SprayingWorkDto : FieldWorkDto
    {
        public ICollection<SprayingWorkCropProtectionProductDto> SprayingWorkCropProtectionProducts { get; set; } = new List<SprayingWorkCropProtectionProductDto>();
    }
    public class HarvestingWorkDto : FieldWorkDto
    {
        public decimal TotalYield { get; set; }
        public decimal? Moisture { get; set; }
        public required CropDto Crop { get; set; }
        public required string unitName { get; set; }
    }
    public class OtherWorkDto : FieldWorkDto
    {

    }
}
