using codetheory.DAL.Models;
using codetheory.DAL.Repositories.Interfaces;

namespace codetheory.DAL.Repositories.Impl
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly CodeTheoryContext _context;
        public QuestionRepository(CodeTheoryContext context)
        {
            _context = context;
        }
        public Question Delete(int id)
        {
            var content = GetById(id);
            if (content == null)
            {
                throw new ArgumentException($"Question with id {id} not found.");
            }

            _context.Questions.Remove(content);
            Save();
            return content;
        }

        public IEnumerable<Question> GetAll()
        {
            return _context.Questions.ToList();
        }

        public Question? GetById(int id)
        {
            return _context.Questions.Find(id);
        }

        public IEnumerable<Question> GetByLessonId(int lessonId)
        {
            return _context.Questions
                .Where(c => c.LessonId == lessonId)
                .OrderBy(c => c.QuestionOrder)
                .ToList();
        }

        public void Insert(Question entity)
        {
            _context.Questions.Add(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Question entity)
        {
            _context.Questions.Update(entity);
        }
    }
}
