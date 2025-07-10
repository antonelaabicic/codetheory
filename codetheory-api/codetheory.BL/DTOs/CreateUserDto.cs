namespace codetheory.BL.DTOs
{
    public class CreateUserDto
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? ImagePath { get; set; } = null!;
        public int RoleId { get; set; }
    }
}
