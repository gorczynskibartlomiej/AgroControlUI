using AgroControlUI.Models.FarmData;
using AgroControlUI.Models.FieldWorks;
using Microsoft.AspNetCore.Identity;

namespace AgroControlUI.Models.UserManagement
{
    public class AgroControlUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<FieldWork> FieldWorks { get; set; } = new List<FieldWork>();
        //public bool FirstLogin { get; set; }
    }
}
