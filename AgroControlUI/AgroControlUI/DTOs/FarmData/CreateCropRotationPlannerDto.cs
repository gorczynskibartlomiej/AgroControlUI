namespace AgroControlUI.DTOs.FarmData
{
    public class CreateCropRotationPlannerDto
    {
        public int Id { get; set; }
        public int FieldId { get; set; }
        public int CropId { get; set; }
        public int? Year { get; set; }

    }
}
