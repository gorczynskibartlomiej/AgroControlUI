using AgroControlUI.Models.FarmData;
using AgroControlUI.Models.ReferenceData;

namespace AgroControlUI.DTOs.UserManagement
{
    public class AgroControlUserFarmDto
    {
        public required int FarmId { get; set; }
        public required string RoleName { get; set; }
        public required string FarmName { get; set; }
    }
}
