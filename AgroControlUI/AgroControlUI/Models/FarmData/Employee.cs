using AgroControlUI.Models.FieldWorks;
using AgroControlUI.Models.ReferenceData;
using AgroControlUI.Models.UserManagement;
using Microsoft.AspNetCore.Identity;

namespace AgroControlUI.Models.FarmData
{
    public class Employee
    {
        public int Id { get; set; }
        public int FarmId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public int? CostUnitId { get; set; }
        public decimal? Cost { get; set; }
        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }

        public required CostUnit CostUnit { get; set; }
        public required Farm Farm { get; set; }

        public ICollection<FieldWork> FieldWorks { get; set; } = new List<FieldWork>();
    }
}
