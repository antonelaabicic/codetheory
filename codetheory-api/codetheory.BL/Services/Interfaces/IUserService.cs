using codetheory.BL.DTOs;
using codetheory.DAL.Models;

namespace codetheory.BL.Services.Interfaces
{
    public interface IUserService
    {
        IEnumerable<UserDto> GetAllUsers();
        IEnumerable<UserDto> GetUsersByRoleId(int roleId);
        UserDto? GetUserById(int id);
        void AddUser(CreateUserDto userDto);
        void UpdateUser(int id, UserDto userDto);
        void DeleteUser(int id);
        UserDto? GetUserByUsername(string username);
    }
}
