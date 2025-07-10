using codetheory.DAL.Models;

namespace codetheory.DAL.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<User> GetUsersByRoleId(int roleId);
    }
}
