using AgroControlUI.Models.FarmData;
using AgroControlUI.Models.ReferenceData;
using Microsoft.AspNetCore.Identity;

namespace AgroControlUI.Models.UserManagement
{
    public class AgroControlUserRole
    {
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public int? CostUnitId { get; set; }
        public decimal? Cost { get; set; }
        public int FarmId { get; set; }

        public required Farm Farm { get; set; }
        public required CostUnit CostUnit { get; set; }
        public required AgroControlUser AgroControlUser { get; set; }
        public required AgroControlRole AgroControlRole { get; set; }
    }
}
