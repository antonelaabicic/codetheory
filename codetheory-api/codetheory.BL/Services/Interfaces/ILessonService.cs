using codetheory.BL.DTOs;

namespace codetheory.BL.Services.Interfaces
{
    public interface ILessonService
    {
        IEnumerable<LessonDTO> GetAllLessons();
        LessonDTO? GetLessonById(int id);
        void AddLesson(LessonDTO lesson);
        void UpdateLesson(int id, LessonDTO updatedLesson);
        void DeleteLesson(int id);
    }
}
