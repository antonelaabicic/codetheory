using AutoMapper;
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
        private readonly IMapper _mapper;
        public UserProgressService(IRepositoryFactory repositoryFactory, IMapper mapper)
        {
            _progressRepository = repositoryFactory.GetRepository<IUserProgressRepository>();
            _userAnswerRepository = repositoryFactory.GetRepository<IUserAnswerRepository>();
            _mapper = mapper;
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

        public IEnumerable<UserProgressDto> GetProgressPerUser(int userId)
        {
            var progressList = _progressRepository.GetByUser(userId);
            return _mapper.Map<IEnumerable<UserProgressDto>>(progressList);
        }

        public UserProgressDto? GetProgress(int userId, int lessonId)
        {
            var progress = _progressRepository.Get(userId, lessonId);
            return progress == null ? null : _mapper.Map<UserProgressDto>(progress);
        }
    }
}
