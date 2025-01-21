namespace AgroControlUI.DTOs.FarmData
{
    public class CropRotationPLannerDetailsDto
    {
        public int Id { get; set; }
        public int FieldId { get; set; }
        public required string FieldName { get; set; }
        public required string CropName { get; set; }
        public string? Description { get; set; }
        public int Year { get; set; }

        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
    }
}
