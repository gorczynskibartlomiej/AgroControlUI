
using AgroControlUI.DTOs.ReferenceData;
using AgroControlUI.Models.ReferenceData;

namespace AgroControlUI.DTOs.FarmData
{
    public class AgriculturalEquipmentDetailsDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Brand { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int? YearOfManufacture { get; set; }
        public int? FuelCapacity { get; set; }
        public int? EnginePower { get; set; }
        public int? Weight { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public decimal? WorkingSpeed { get; set; }
        public decimal? TransportSpeed { get; set; }
        public decimal? WorkingWidth { get; set; }
        public decimal? Cost { get; set; }
        public string? CostUnitName { get; set; }
        public DateOnly? LastServiceDate { get; set; }
        public DateOnly? NextServiceDate { get; set; }

        public DateTime CreatedOn { get; set; }
        public required string CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public required string UpdatedBy { get; set; }

        public FuelDto? Fuel { get; set; }
        public AgriculturalEquipmentTypeDto? AgriculturalEquipmentType { get; set; }
    }
}
