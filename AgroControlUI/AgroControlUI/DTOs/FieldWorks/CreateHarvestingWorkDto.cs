using AgroControlUI.DTOs.ReferenceData;

namespace AgroControlUI.DTOs.FieldWorks
{
    public class CreateHarvestingWorkDto
    {
        public int Id { get; set; }
        public int FieldId { get; set; }
        public bool IsPlanned { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? EmployeeId { get; set; }
        public int? AgroControlUserId { get; set; }
        public string? Description { get; set; }

        public ICollection<int>? FieldWorkAgriculturalEquipmentIds { get; set; } = new List<int>();
        public decimal? TotalYield { get; set; }
        public decimal? Moisture { get; set; }
        public int? CropId { get; set; }
    }
}
