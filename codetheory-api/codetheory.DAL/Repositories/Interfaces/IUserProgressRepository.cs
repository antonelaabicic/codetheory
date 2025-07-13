using codetheory.DAL.Models;

namespace codetheory.DAL.Repositories.Interfaces
{
    public interface IUserProgressRepository
    {
        UserProgress? Get(int userId, int lessonId);
        void Upsert(UserProgress progress);
        void Save();
    }
}
