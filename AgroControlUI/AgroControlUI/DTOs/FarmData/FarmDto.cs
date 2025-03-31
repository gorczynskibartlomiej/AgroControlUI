using AgroControlUI.Models.FarmData;
using AgroControlUI.Models.UserManagement;

namespace AgroControlUI.DTOs.FarmData
{
    public class FarmDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? FarmNumber { get; set; }
        public string? Description { get; set; }
        public decimal TotalArea { get; set; }
        public int YearEstablished { get; set; }
        public  string? Adress { get; set; }
        public  string? PostalCode { get; set; }
        public string? City { get; set; }

        public DateTime CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
