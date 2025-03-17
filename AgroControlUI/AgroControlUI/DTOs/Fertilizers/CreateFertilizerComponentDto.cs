namespace AgroControlUI.DTOs.Fertilizers
{
    public class CreateFertilizerComponentDto
    {
        public string ElementPercentage { get; set; }
        public required int ChemicalElementId { get; set; }
    }
}
