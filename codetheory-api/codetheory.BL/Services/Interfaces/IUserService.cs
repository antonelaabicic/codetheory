using codetheory.BL.DTOs;

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
    }
}
