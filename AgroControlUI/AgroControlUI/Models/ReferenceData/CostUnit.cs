using AgroControlUI.Models.FarmData;
using AgroControlUI.Models.UserManagement;

namespace AgroControlUI.Models.ReferenceData
{
    public class CostUnit
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public ICollection<AgriculturalEquipment> AgriculturalEquipment { get; set; } = new List<AgriculturalEquipment>();
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public ICollection<AgroControlUserRole> AgroControlUserRoles { get; set; } = new List<AgroControlUserRole>();

    }
}
