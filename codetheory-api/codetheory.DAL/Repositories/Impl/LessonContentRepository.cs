using codetheory.DAL.Models;
using codetheory.DAL.Repositories.Interfaces;

namespace codetheory.DAL.Repositories.Impl
{
    public class LessonContentRepository : ILessonContentRepository
    {
        private readonly CodeTheoryContext _context;
        public LessonContentRepository(CodeTheoryContext context)
        {
            _context = context;
        }
        public LessonContent Delete(int id)
        {
            var content = GetById(id);
            if (content == null)
            {
                throw new ArgumentException($"Lesson content with id {id} not found.");
            }

            _context.LessonContents.Remove(content);
            Save();
            return content;
        }

        public IEnumerable<LessonContent> GetAll()
        {
            return _context.LessonContents.ToList();
        }

        public LessonContent? GetById(int id)
        {
            return _context.LessonContents.Find(id);
        }

        public IEnumerable<LessonContent> GetByLessonId(int lessonId)
        {
            return _context.LessonContents
                .Where(c => c.LessonId == lessonId)
                .OrderBy(c => c.ContentOrder)
                .ToList();
        }

        public void Insert(LessonContent entity)
        {
            _context.LessonContents.Add(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(LessonContent entity)
        {
            _context.LessonContents.Update(entity);
        }
    }
}
