using codetheory.BL.DTOs;
using codetheory.BL.Services.Interfaces;
using codetheory.DAL.Config;
using codetheory.DAL.Models;
using codetheory.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace codetheory.BL.Services.Impl
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(IRepositoryFactory repositoryFactory, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = repositoryFactory.GetRepository<IUserRepository>();
            _passwordHasher = passwordHasher;
        }

        public string Login(LoginDto loginDto)
        {
            var user = _userRepository.GetByUsername(loginDto.Username);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
            if (result != PasswordVerificationResult.Success)
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var key = Encoding.UTF8.GetBytes(ConfigManager.JwtSecret);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role?.Name ?? "User")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256
                )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

    }
}
