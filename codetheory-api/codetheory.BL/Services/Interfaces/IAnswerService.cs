using codetheory.BL.DTOs;

namespace codetheory.BL.Services.Interfaces
{
    public interface IAnswerService
    {
        IEnumerable<AnswerDto> GetAnswersByQuestionId(int questionId);
    }
}
