using AgroControlUI.Models.CropProtectionProducts;
using AgroControlUI.Models.Fertilizers;

namespace AgroControlUI.Models.ReferenceData
{
    public class Producer
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public ICollection<Fertilizer> Fertilizers { get; set; } = new List<Fertilizer>();
        public ICollection<Seed> Seeds { get; set; } = new List<Seed>();
        public ICollection<CropProtectionProduct> CropProtectionProducts { get; set; } = new List<CropProtectionProduct>();
    }
}
