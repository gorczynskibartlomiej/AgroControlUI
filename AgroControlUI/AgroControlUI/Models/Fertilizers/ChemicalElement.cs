namespace AgroControlUI.Models.Fertilizers
{
    public class ChemicalElement
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string ChemicalSymbol { get; set; }

        public ICollection<FertilizerComponent> FertilizerComponents { get; set; } = new List<FertilizerComponent>();
    }
}
