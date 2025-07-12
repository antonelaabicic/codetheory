namespace codetheory.DAL.Models;
public partial class UserProgress
{
    public int UserId { get; set; }

    public int LessonId { get; set; }

    public bool? IsCompleted { get; set; }

    public decimal? Score { get; set; }

    public DateTime? CompletedAt { get; set; }

    public virtual Lesson Lesson { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
