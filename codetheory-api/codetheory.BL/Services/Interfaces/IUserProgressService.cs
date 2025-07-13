using codetheory.BL.DTOs;

namespace codetheory.BL.Services.Interfaces
{
    public interface IUserProgressService
    {
        void EvaluateAndSaveProgress(int userId, int lessonId);
        UserProgressDto? GetProgress(int userId, int lessonId);
    }
}
