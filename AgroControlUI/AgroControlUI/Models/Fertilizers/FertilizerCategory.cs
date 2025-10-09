using Microsoft.Identity.Client;

namespace AgroControlUI.Models.Fertilizers
{
    public class FertilizerCategory
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public ICollection<Fertilizer> Fertilizers { get; set; } = new List<Fertilizer>();
    }
}
