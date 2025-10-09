using AgroControlUI.Models.Fertilizers;

namespace AgroControlUI.DTOs.Fertilizers
{
    public class FertilizerComponentDto
    {
        public decimal ElementPercentage { get; set; }
        public required ChemicalElementDto ChemicalElement { get; set; }
    }
}
