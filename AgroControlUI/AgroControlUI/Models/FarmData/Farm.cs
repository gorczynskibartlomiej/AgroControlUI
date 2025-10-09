using AgroControlUI.Models.FieldWorks;
using AgroControlUI.Models.ReferenceData;
using AgroControlUI.Models.UserManagement;

namespace AgroControlUI.Models.FarmData
{
    public class Farm
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? FarmNumber { get; set; }
        public string? Description { get; set; }
        public decimal TotalArea { get; set; }
        public int YearEstablished { get; set; }
        public required string Adress { get; set; }
        public required string PostalCode { get; set; }
        public required string City { get; set; }

        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }

        public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
        public ICollection<Field> Fields { get; set; } = new List<Field>();
        public ICollection<AgriculturalEquipment> AgriculturalEquipment { get; set; } = new List<AgriculturalEquipment>();
        //public ICollection<FarmCropProtectionProduct> FarmCropProtectionProducts { get; set; } = new List<FarmCropProtectionProduct>();
        //public ICollection<FarmFertilizer> FarmFertilizers { get; set; } = new List<FarmFertilizer>();
        //public ICollection<FarmFuel> FarmFuels { get; set; } = new List<FarmFuel>();
        //public ICollection<FarmSeed> FarmSeeds { get; set; } = new List<FarmSeed>();
        public ICollection<FarmInventory> FarmInventories { get; set; } = new List<FarmInventory>();
        public ICollection<AgroControlUserRole> AgroControlUserRoles { get; set; } = new List<AgroControlUserRole>();
        public ICollection<GrainSilo> GrainSilos { get; set; } = new List<GrainSilo>();
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
