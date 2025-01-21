using AgroControlUI.Models.ReferenceData;

namespace AgroControlUI.Models.FarmData
{
    public class CropRotationPlanner
    {
        public int Id { get; set; }
        public int FieldId { get; set; }
        public int CropId { get; set; }
        public string? Description { get; set; }
        public int Year { get; set; }

        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }

        public required Field Field { get; set; }
        public required Crop Crop { get; set; }

    }
}
