using codetheory.BL.DTOs;

namespace codetheory.BL.Services.Interfaces
{
    public interface IUserAnswerService
    {
        IEnumerable<UserAnswerDto> GetByUser(int userId);
        UserAnswerDto? GetByUserAndAnswer(int userId, int answerId);
        IEnumerable<UserAnswerDto> GetByUserAndLesson(int userId, int lessonId);
        void SubmitAnswers(IEnumerable<UserAnswerDto> answers);
        void UpdateAnswers(IEnumerable<UserAnswerDto> answers);
    }
}
