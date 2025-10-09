namespace AgroControlUI.DTOs.UserManagement
{
    public class AgroControlUserDto
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string RoleName { get; set; }
    }
}
