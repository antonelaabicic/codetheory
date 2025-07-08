using codetheory.DAL.Models;

namespace codetheory.BL.DTOs
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public int QuestionOrder { get; set; }
        public List<AnswerDto> Answers { get; set; } = new();
    }
}
