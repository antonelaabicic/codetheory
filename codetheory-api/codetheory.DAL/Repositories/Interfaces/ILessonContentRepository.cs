using codetheory.DAL.Models;

namespace codetheory.DAL.Repositories.Interfaces
{
    public interface ILessonContentRepository : IRepository<LessonContent>
    {
        IEnumerable<LessonContent> GetByLessonId(int lessonId);
    }
}
