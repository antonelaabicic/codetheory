namespace codetheory.Web.Models
{
    public class UserProgressDto
    {
        public int UserId { get; set; }
        public int LessonId { get; set; }
        public decimal Score { get; set; }
        public bool IsCompleted { get; set; }
    }
}
