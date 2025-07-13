using codetheory.DAL.Models;
using codetheory.DAL.Repositories.Interfaces;

namespace codetheory.DAL.Repositories.Impl
{
    public class UserProgressRepository : IUserProgressRepository
    {
        private readonly CodeTheoryContext _context;
        public UserProgressRepository(CodeTheoryContext context)
        {
            _context = context;
        }
        public UserProgress? Get(int userId, int lessonId)
        {
            return _context.UserProgresses.FirstOrDefault(up => up.UserId == userId && up.LessonId == lessonId);
        }

        public IEnumerable<UserProgress> GetByUser(int userId)
        {
            return _context.UserProgresses.Where(p => p.UserId == userId).ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Upsert(UserProgress progress)
        {
            var existing = Get(progress.UserId, progress.LessonId);

            if (existing != null)
            {
                existing.Score = progress.Score;
                existing.IsCompleted = progress.IsCompleted;
            }
            else
            {
                _context.UserProgresses.Add(progress);
            }
        }
    }
}
