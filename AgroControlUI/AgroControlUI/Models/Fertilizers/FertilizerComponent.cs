namespace AgroControlUI.Models.Fertilizers
{
    public class FertilizerComponent
    {
        public int FertilizerId { get; set; }
        public int ChemicalElementId { get; set; }
        public decimal ElementPercentage { get; set; }

        public required Fertilizer Fertilizer { get; set; }
        public required ChemicalElement ChemicalElement { get; set; }
    }

}
