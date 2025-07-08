using codetheory.BL.DTOs;

namespace codetheory.BL.Services.Interfaces
{
    public interface IQuestionService
    {
        IEnumerable<QuestionDto> GetAllQuestions();
        QuestionDto? GetQuestionById(int id);
        IEnumerable<QuestionDto> GetQuestionsByLessonId(int lessonId);
    }
}
