namespace codetheory.BL.DTOs
{
    public class AnswerDto
    {
        public int Id { get; set; }
        public string AnswerText { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }
}
