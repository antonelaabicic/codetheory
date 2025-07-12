using codetheory.DAL.Models;

namespace codetheory.DAL.Repositories.Interfaces
{
    public interface IUserAnswerRepository
    {
        IEnumerable<UserAnswer> GetByUser(int userId);
        UserAnswer? GetByUserAndAnswer(int userId, int answerId);
        IEnumerable<UserAnswer> GetByUserAndLesson(int userId, int lessonId);
        void InsertMany(IEnumerable<UserAnswer> answers);
        void UpdateMany(IEnumerable<UserAnswer> answers);
        void Save();
    }
}
