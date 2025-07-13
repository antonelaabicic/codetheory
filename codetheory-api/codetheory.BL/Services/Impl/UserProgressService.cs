using codetheory.BL.DTOs;
using codetheory.BL.Services.Interfaces;
using codetheory.DAL.Models;
using codetheory.DAL.Repositories.Interfaces;

namespace codetheory.BL.Services.Impl
{
    public class UserProgressService : IUserProgressService
    {
        private readonly IUserProgressRepository _progressRepository;
        private readonly IUserAnswerRepository _userAnswerRepository;
        public UserProgressService(IRepositoryFactory repositoryFactory)
        {
            _progressRepository = repositoryFactory.GetRepository<IUserProgressRepository>();
            _userAnswerRepository = repositoryFactory.GetRepository<IUserAnswerRepository>();
        }
        public void EvaluateAndSaveProgress(int userId, int lessonId)
        {
            var userAnswers = _userAnswerRepository.GetByUserAndLesson(userId, lessonId).ToList();
            if (!userAnswers.Any())
            {
                Console.WriteLine("No answers found for evaluation.");
                return;
            }

            var total = userAnswers.Count;
            var correct = userAnswers.Count(a => a.Answer.IsCorrect);

            var score = Math.Round((decimal)correct / total * 100, 2);
            var isCompleted = score >= 50;

            var progress = new UserProgress
            {
                UserId = userId,
                LessonId = lessonId,
                Score = score,
                IsCompleted = isCompleted
            };

            _progressRepository.Upsert(progress);
            _progressRepository.Save();

            Console.WriteLine($"Progress evaluated: Score = {score}, Completed = {isCompleted}");
        }

        public UserProgressDto? GetProgress(int userId, int lessonId)
        {
            var progress = _progressRepository.Get(userId, lessonId);
            if (progress == null) return null;

            return new UserProgressDto
            {
                UserId = progress.UserId,
                LessonId = progress.LessonId,
                Score = progress.Score ?? 0,
                IsCompleted = progress.IsCompleted ?? false
            };
        }
    }
}
