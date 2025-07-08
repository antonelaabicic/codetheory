using codetheory.DAL.Models;
using codetheory.DAL.Repositories.Interfaces;

namespace codetheory.DAL.Repositories.Impl
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly CodeTheoryContext _context;
        public AnswerRepository(CodeTheoryContext context)
        {
            _context = context;
        }
        public Answer Delete(int id)
        {
            var content = GetById(id);
            if (content == null)
            {
                throw new ArgumentException($"Answer with id {id} not found.");
            }

            _context.Answers.Remove(content);
            Save();
            return content;
        }

        public IEnumerable<Answer> GetAll()
        {
            return _context.Answers.ToList();
        }

        public Answer? GetById(int id)
        {
            return _context.Answers.Find(id);
        }

        public IEnumerable<Answer> GetByQuestionId(int questionId)
        {
            return _context.Answers
                .Where(c => c.QuestionId == questionId)
                .ToList();
        }

        public void Insert(Answer entity)
        {
            _context.Answers.Add(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Answer entity)
        {
            _context.Answers.Update(entity);
        }
    }
}
