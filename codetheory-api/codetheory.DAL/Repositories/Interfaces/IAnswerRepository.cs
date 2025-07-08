using codetheory.DAL.Models;

namespace codetheory.DAL.Repositories.Interfaces
{
    public interface IAnswerRepository : IRepository<Answer>
    {
        IEnumerable<Answer> GetByQuestionId(int questionId);
    }
}
