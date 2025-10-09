namespace AgroControlUI.DTOs.Fertilizers
{
    public class CreateFertilizerComponentWithStringDto
    {
        public string ElementPercentage { get; set; }
        public required int ChemicalElementId { get; set; }
    }
}
