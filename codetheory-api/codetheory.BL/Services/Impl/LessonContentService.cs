using AutoMapper;
using codetheory.BL.DTOs;
using codetheory.BL.Services.Interfaces;
using codetheory.BL.Validation;
using codetheory.DAL.Models;
using codetheory.DAL.Repositories.Interfaces;

namespace codetheory.BL.Services.Impl
{
    public class LessonContentService : ILessonContentService
    {
        private readonly ILessonContentRepository _lessonContentRepository;
        private readonly IMapper _mapper;
        public LessonContentService(IRepositoryFactory repositoryFactory, IMapper mapper)
        {
            _lessonContentRepository = repositoryFactory.GetRepository<ILessonContentRepository>();
            _mapper = mapper;
        }
        public void AddLessonContent(LessonContentDto contentDto)
        {
            var entity = _mapper.Map<LessonContent>(contentDto);
            LessonContentValidator.ParseAndValidateContentData(entity);

            _lessonContentRepository.Insert(entity);
            _lessonContentRepository.Save();
        }

        public void DeleteLessonContent(int id)
        {
            _lessonContentRepository.Delete(id);
            _lessonContentRepository.Save();
        }

        public LessonContentDto? GetContentById(int id)
        {
            var content = _lessonContentRepository.GetById(id);
            return content == null ? null : _mapper.Map<LessonContentDto>(content);
        }

        public IEnumerable<LessonContentDto> GetContentsByLessonId(int lessonId)
        {
            var contents = _lessonContentRepository.GetByLessonId(lessonId);
            return _mapper.Map<IEnumerable<LessonContentDto>>(contents);
        }

        public void UpdateLessonContent(int id, LessonContentDto updatedContentDto)
        {
            var existing = _lessonContentRepository.GetById(id);
            if (existing == null)
                throw new ArgumentException("Content not found.");

            existing.ContentData = updatedContentDto.ContentData;
            existing.ContentOrder = updatedContentDto.ContentOrder;
            existing.ContentTypeId = updatedContentDto.ContentTypeId;
            existing.LessonId = updatedContentDto.LessonId;

            LessonContentValidator.ParseAndValidateContentData(existing);

            _lessonContentRepository.Update(existing);
            _lessonContentRepository.Save();
        }
    }
}
