using codetheory.DAL.Models;
using codetheory.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace codetheory.DAL.Repositories.Impl
{
    public class UserRepository : IUserRepository
    {
        private readonly CodeTheoryContext _context;
        public UserRepository(CodeTheoryContext context)
        {
            _context = context;
        }

        public User Delete(int id)
        {
            var content = GetById(id);
            if (content == null)
            {
                throw new ArgumentException($"User with id {id} not found.");
            }

            _context.Users.Remove(content);
            Save();
            return content;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User? GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public User? GetByUsername(string username)
        {
            return _context.Users.Include(u => u.Role).FirstOrDefault(u => u.Username == username);
        }

        public IEnumerable<User> GetUsersByRoleId(int roleId)
        {
            return _context.Users.Where(u => u.RoleId == roleId).ToList();
        }

        public void Insert(User entity)
        {
            _context.Users.Add(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(User entity)
        {
            _context.Users.Update(entity);
        }
    }
}
