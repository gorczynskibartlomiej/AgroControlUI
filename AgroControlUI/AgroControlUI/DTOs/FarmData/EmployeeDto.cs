using AgroControlUI.Models.FarmData;
using AgroControlUI.Models.FieldWorks;
using AgroControlUI.Models.ReferenceData;

namespace AgroControlUI.DTOs.FarmData
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? CostUnitName { get; set; }
        public decimal? Cost { get; set; }
        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
    }
}
