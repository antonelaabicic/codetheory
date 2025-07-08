using AutoMapper;
using codetheory.BL.DTOs;
using codetheory.BL.Services.Interfaces;
using codetheory.DAL.Repositories.Impl;
using codetheory.DAL.Repositories.Interfaces;

namespace codetheory.BL.Services.Impl
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper _mapper;
        public QuestionService(IRepositoryFactory repositoryFactory, IMapper mapper)
        {
            _questionRepository = repositoryFactory.GetRepository<IQuestionRepository>();
            _mapper = mapper;
        }
        public IEnumerable<QuestionDto> GetAllQuestions()
        {
            var questions = _questionRepository.GetAll();
            return _mapper.Map<IEnumerable<QuestionDto>>(questions);
        }

        public QuestionDto? GetQuestionById(int id)
        {
            var question = _questionRepository.GetById(id);
            return question == null ? null : _mapper.Map<QuestionDto>(question);
        }

        public IEnumerable<QuestionDto> GetQuestionsByLessonId(int lessonId)
        {
            var questions = _questionRepository.GetByLessonId(lessonId);
            return _mapper.Map<IEnumerable<QuestionDto>>(questions);
        }
    }
}
