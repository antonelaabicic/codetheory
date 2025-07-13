using codetheory.DAL.Models;

namespace codetheory.DAL.Repositories.Interfaces
{
    public interface IUserProgressRepository
    {
        UserProgress? Get(int userId, int lessonId);
        IEnumerable<UserProgress> GetByUser(int userId);
        void Upsert(UserProgress progress);
        void Save();
    }
}
