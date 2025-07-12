using AutoMapper;
using codetheory.BL.DTOs;
using codetheory.BL.Services.Interfaces;
using codetheory.DAL.Models;
using codetheory.DAL.Repositories.Interfaces;

namespace codetheory.BL.Services.Impl
{
    public class UserAnswerService : IUserAnswerService
    {
        private readonly IUserAnswerRepository _repository;
        private readonly IMapper _mapper;
        public UserAnswerService(IUserAnswerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public IEnumerable<UserAnswerDto> GetByUser(int userId)
        {
            var entities = _repository.GetByUser(userId);
            return _mapper.Map<IEnumerable<UserAnswerDto>>(entities);
        }

        public UserAnswerDto? GetByUserAndAnswer(int userId, int answerId)
        {
            var entity = _repository.GetByUserAndAnswer(userId, answerId);
            return entity == null ? null : _mapper.Map<UserAnswerDto>(entity);
        }

        public IEnumerable<UserAnswerDto> GetByUserAndLesson(int userId, int lessonId)
        {
            var data = _repository.GetByUserAndLesson(userId, lessonId);
            return _mapper.Map<IEnumerable<UserAnswerDto>>(data);
        }

        public void SubmitAnswers(IEnumerable<UserAnswerDto> answers)
        {
            var entities = _mapper.Map<IEnumerable<UserAnswer>>(answers);
            _repository.InsertMany(entities);
            _repository.Save();
        }

        public void UpdateAnswers(IEnumerable<UserAnswerDto> answers)
        {
            var entities = _mapper.Map<IEnumerable<UserAnswer>>(answers);
            _repository.UpdateMany(entities);
            _repository.Save();
        }
    }
}
