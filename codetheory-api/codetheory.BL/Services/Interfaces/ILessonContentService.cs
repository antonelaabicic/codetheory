using codetheory.BL.DTOs;

namespace codetheory.BL.Services.Interfaces
{
    public interface ILessonContentService
    {
        IEnumerable<LessonContentDto> GetContentsByLessonId(int lessonId);
        LessonContentDto? GetContentById(int id);
        void AddLessonContent(LessonContentDto contentDto);
        void UpdateLessonContent(int id, LessonContentDto updatedContentDto);
        void DeleteLessonContent(int id);
    }
}
