using System.Reflection.PortableExecutable;
using AgroControlUI.Models.FarmData;
using AgroControlUI.Models.ReferenceData;
using AgroControlUI.Models.UserManagement;

namespace AgroControlUI.Models.FieldWorks
{
    public class FieldWork
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

        public ICollection<FieldWorkAgriculturalEquipment> FieldWorkAgriculturalEquipment { get; set; } = new List<FieldWorkAgriculturalEquipment>();
        public required Field Field { get; set; }
        public Employee? Employee { get; set; }
        public AgroControlUser? AgroControlUser { get; set; }
    }
    public class FertilizingWork : FieldWork
    {
        public ICollection<FertilizingWorkFertilizer> FertilizingWorkFertilizers { get; set; } = new List<FertilizingWorkFertilizer>();
    }
    public class SprayingWork : FieldWork
    {
        public ICollection<SprayingWorkCropProtectionProduct> SprayingWorkCropProtectionProducts { get; set; } = new List<SprayingWorkCropProtectionProduct>();
    }
    public class HarvestingWork : FieldWork
    {
        public decimal TotalYield { get; set; }
        public decimal? Moisture { get; set; }
        public int UnitId { get; set; }
        public int CropId { get; set; }
        public required Crop Crop { get; set; }
        public required Unit Unit { get; set; }
    }
    public class OtherWork : FieldWork
    {

    }
}
