using codetheory.DAL.Models;

namespace codetheory.DAL.Repositories.Interfaces
{
    public interface IQuestionRepository : IRepository<Question>
    {
        IEnumerable<Question> GetByLessonId(int lessonId);
    }
}
