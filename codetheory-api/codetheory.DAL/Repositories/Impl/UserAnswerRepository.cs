using codetheory.DAL.Models;
using codetheory.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace codetheory.DAL.Repositories.Impl
{
    public class UserAnswerRepository : IUserAnswerRepository
    {
        private readonly CodeTheoryContext _context;
        public UserAnswerRepository(CodeTheoryContext context)
        {
            _context = context;
        }

        public IEnumerable<UserAnswer> GetByUser(int userId)
        {
            return _context.UserAnswers
                .Include(ua => ua.Answer)
                .ThenInclude(a => a.Question)
                .Where(ua => ua.UserId == userId)
                .ToList();
        }

        public UserAnswer? GetByUserAndAnswer(int userId, int answerId)
        {
            return _context.UserAnswers.FirstOrDefault(ua => ua.UserId == userId && ua.AnswerId == answerId);
        }

        public IEnumerable<UserAnswer> GetByUserAndLesson(int userId, int lessonId)
        {
            return _context.UserAnswers
                .Include(ua => ua.Answer)
                .ThenInclude(a => a.Question)
                .Where(ua => ua.UserId == userId && ua.Answer != null && 
                    ua.Answer.Question != null && ua.Answer.Question.LessonId == lessonId)
                .ToList();
        }

        public void InsertMany(IEnumerable<UserAnswer> answers)
        {
            _context.UserAnswers.AddRange(answers);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void UpdateMany(IEnumerable<UserAnswer> newAnswers)
        {
            foreach (var newAnswer in newAnswers)
            {
                var questionId = _context.Answers
                    .Where(a => a.Id == newAnswer.AnswerId)
                    .Select(a => a.QuestionId)
                    .FirstOrDefault();

                if (questionId == 0)
                {
                    continue;
                }

                var oldUserAnswer = _context.UserAnswers
                    .Include(ua => ua.Answer)
                    .FirstOrDefault(ua =>
                        ua.UserId == newAnswer.UserId &&
                        ua.Answer.QuestionId == questionId);

                if (oldUserAnswer != null)
                {
                    _context.UserAnswers.Remove(oldUserAnswer); 
                }

                _context.UserAnswers.Add(newAnswer); 
            }
        }
    }
}
