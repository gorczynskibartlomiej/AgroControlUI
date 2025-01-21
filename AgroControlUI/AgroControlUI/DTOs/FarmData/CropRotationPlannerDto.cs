namespace AgroControlUI.DTOs.FarmData
{
    public class CropRotationPlannerDto
    {
        public int Id { get; set; }
        public int FieldId { get; set; }
        public required string FieldName { get; set; }
        public required string CropName { get; set; }
        public string? Description { get; set; }
        public int Year { get; set; }
    }
}
