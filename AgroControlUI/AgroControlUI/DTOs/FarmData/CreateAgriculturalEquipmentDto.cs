namespace AgroControlUI.DTOs.FarmData
{
    public class CreateAgriculturalEquipmentDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Brand { get; set; }
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
        public string? CostUnitName { get; set; }
        public DateOnly? LastServiceDate { get; set; }
        public DateOnly? NextServiceDate { get; set; }
        public int AgriculturalEquipmentTypeId { get; set; }
    }
}
