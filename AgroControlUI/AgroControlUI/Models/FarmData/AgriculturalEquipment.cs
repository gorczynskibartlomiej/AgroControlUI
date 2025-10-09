using AgroControlUI.Models.FieldWorks;
using AgroControlUI.Models.ReferenceData;
using AgroControlUI.Models.UserManagement;

namespace AgroControlUI.Models.FarmData
{
    public class AgriculturalEquipment
    {
        public int Id { get; set; }
        public int FarmId { get; set; }
        public required string Name { get; set; }
        public required string Brand { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int? YearOfManufacture { get; set; }
        public int? FuelId { get; set; }
        public int? FuelCapacity { get; set; }
        public int? EnginePower { get; set; }
        public int? Weight { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public decimal? WorkingSpeed { get; set; }
        public decimal? TransportSpeed { get; set; }
        public decimal? WorkingWidth { get; set; }
        public decimal? Cost { get; set; }
        public int? CostUnitId { get; set; }
        public DateOnly? LastServiceDate { get; set; }
        public DateOnly? NextServiceDate { get; set; }
        public int AgriculturalEquipmentTypeId { get; set; }

        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }

        public Fuel? Fuel { get; set; }
        public required AgriculturalEquipmentType AgriculturalEquipmentType { get; set; }
        public required Farm Farm { get; set; }
        public CostUnit? CostUnit { get; set; }

        public ICollection<FieldWorkAgriculturalEquipment> FieldWorkAgriculturalEquipment { get; set; } = new List<FieldWorkAgriculturalEquipment>();
    }
}
