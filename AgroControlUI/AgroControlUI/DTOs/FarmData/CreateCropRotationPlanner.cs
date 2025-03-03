namespace AgroControlUI.DTOs.FarmData
{
    public class CreateCropRotationPlanner
    {
        public int Id { get; set; }
        public int FieldId { get; set; }
        public int CropId { get; set; }
        public string? Description { get; set; }
        public int Year { get; set; }

    }
}
