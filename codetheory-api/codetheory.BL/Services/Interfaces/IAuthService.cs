using codetheory.BL.DTOs;

namespace codetheory.BL.Services.Interfaces
{
    public interface IAuthService
    {
        string Login(LoginDto loginDto);
    }
}
