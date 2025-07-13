using codetheory.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace codetheory.DAL.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<User> GetUsersByRoleId(int roleId);
        User? GetByUsername(string username);
        IEnumerable<User> GetStudentsWithProgress();
    }
}
