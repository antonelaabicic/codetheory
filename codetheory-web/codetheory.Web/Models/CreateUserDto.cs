namespace codetheory.Web.Models
{
    public class CreateUserDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? ImagePath { get; set; }
        public int RoleId { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}
