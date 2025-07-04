using codetheory.DAL.Models;
using codetheory.DAL.Repositories.Interfaces;

namespace codetheory.DAL.Repositories.Impl
{
    public class LessonRepository : ILessonRepository
    {
        private readonly CodeTheoryContext _context;
        public LessonRepository(CodeTheoryContext context)
        {
            _context = context;
        }
        public Lesson Delete(int id)
        {
            var lesson = GetById(id);

            if (lesson == null)
            {
                throw new ArgumentException($"Lesson with id {id} not found.");
            }

            _context.Lessons.Remove(lesson);
            return lesson;
        }

        public IEnumerable<Lesson> GetAll()
        {
            return _context.Lessons
                  .OrderBy(l => l.LessonOrder)
                  .ToList();
        }

        public Lesson? GetById(int id)
        {
            return _context.Lessons.Find(id);
        }

        public void Insert(Lesson entity)
        {
            _context.Lessons.Add(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Lesson entity)
        {
            _context.Lessons.Update(entity);
        }
    }
}
