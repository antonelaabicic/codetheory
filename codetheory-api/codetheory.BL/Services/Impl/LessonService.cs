using AutoMapper;
using codetheory.BL.DTOs;
using codetheory.BL.Services.Interfaces;
using codetheory.DAL.Models;
using codetheory.DAL.Repositories.Interfaces;

namespace codetheory.BL.Services.Impl
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IMapper _mapper;
        public LessonService(IRepositoryFactory repositoryFactory, IMapper mapper)
        {
            _lessonRepository = repositoryFactory.GetRepository<ILessonRepository>();
            _mapper = mapper;
        }

        public void AddLesson(LessonDTO lesson)
        {
            var entity = _mapper.Map<Lesson>(lesson);
            _lessonRepository.Insert(entity);
            _lessonRepository.Save();
        }

        public void DeleteLesson(int id)
        {
            _lessonRepository.Delete(id);
            _lessonRepository.Save();
        }

        public IEnumerable<LessonDTO> GetAllLessons()
        {
            var lessons = _lessonRepository.GetAll();
            return _mapper.Map<IEnumerable<LessonDTO>>(lessons);
        }

        public LessonDTO? GetLessonById(int id)
        {
            var lesson = _lessonRepository.GetById(id);
            return lesson == null ? null : _mapper.Map<LessonDTO>(lesson);
        }

        public void UpdateLesson(int id, LessonDTO updatedLesson)
        {
            var lesson = _lessonRepository.GetById(id);
            if (lesson == null)
            {
                throw new ArgumentException("Lesson not found.");
            }

            lesson.Title = updatedLesson.Title;
            lesson.Summary = updatedLesson.Summary;
            lesson.LessonOrder = updatedLesson.LessonOrder;

            _lessonRepository.Update(lesson);
            _lessonRepository.Save();
        }
    }
}
