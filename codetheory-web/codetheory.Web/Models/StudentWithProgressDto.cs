namespace codetheory.Web.Models
{
    public class StudentWithProgressDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string ImagePath { get; set; } = "";
        public List<UserProgressDto> Progress { get; set; } = new();
    }
}
