using AutoMapper;
using codetheory.BL.DTOs;
using codetheory.BL.Services.Interfaces;
using codetheory.DAL.Models;
using codetheory.DAL.Repositories.Impl;
using codetheory.DAL.Repositories.Interfaces;

namespace codetheory.BL.Services.Impl
{
    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IMapper _mapper;
        public AnswerService(IRepositoryFactory repositoryFactory, IMapper mapper)
        {
            _answerRepository = repositoryFactory.GetRepository<IAnswerRepository>();
            _mapper = mapper;
        }
        public IEnumerable<AnswerDto> GetAnswersByQuestionId(int questionId)
        {
            var contents = _answerRepository.GetByQuestionId(questionId);
            return _mapper.Map<IEnumerable<AnswerDto>>(contents);
        }
    }
}
