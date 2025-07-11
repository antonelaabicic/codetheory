using AutoMapper;
using codetheory.BL.DTOs;
using codetheory.BL.Services.Interfaces;
using codetheory.DAL.Config;
using codetheory.DAL.Models;
using codetheory.DAL.Repositories.Impl;
using codetheory.DAL.Repositories.Interfaces;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace codetheory.BL.Services.Impl
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IEncryptionProvider _encryption;
        private readonly IMapper _mapper;

        public UserService(IRepositoryFactory repositoryFactory, IPasswordHasher<User> passwordHasher, IEncryptionProvider encryption, IMapper mapper)
        {
            _userRepository = repositoryFactory.GetRepository<IUserRepository>();
            _passwordHasher = passwordHasher;
            _encryption = encryption;
            _mapper = mapper;
        }

        public void AddUser(CreateUserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);

            user.PasswordHash = _passwordHasher.HashPassword(user, userDto.Password);

            user.Email = _encryption.Encrypt(userDto.Email);
            user.FirstName = _encryption.Encrypt(userDto.FirstName ?? "");
            user.LastName = _encryption.Encrypt(userDto.LastName ?? "");

            var imagePath = string.IsNullOrWhiteSpace(userDto.ImagePath)
                ? ConfigManager.DefaultImagePath
                : userDto.ImagePath;

            user.ImagePath = _encryption.Encrypt(imagePath);

            _userRepository.Insert(user);
            _userRepository.Save();
        }

        public void DeleteUser(int id)
        {
            _userRepository.Delete(id);
            _userRepository.Save();
        }

        public IEnumerable<UserDto> GetAllUsers()
        {
            var users = _userRepository.GetAll();
            foreach (var user in users)
            {
                DecryptSensitiveFields(user);
            }

            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public UserDto? GetUserById(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
            {
                throw new ArgumentException($"User with id {id} not found.");
            }

            DecryptSensitiveFields(user);

            return _mapper.Map<UserDto>(user);
        }

        public void UpdateUser(int id, UserDto userDto)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
            {
                throw new ArgumentException($"User with id {id} not found.");
            }

            user.Username = userDto.Username ?? user.Username;
            EncryptSensitiveFields(user, userDto.Email, userDto.FirstName, userDto.LastName, userDto.ImagePath);
            user.RoleId = userDto.RoleId ?? user.RoleId;

            _userRepository.Update(user);
            _userRepository.Save();
        }

        private void EncryptSensitiveFields(User user, string? email, string? firstName, string? lastName, string? imagePath)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                user.Email = _encryption.Encrypt(email);
            }

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                user.FirstName = _encryption.Encrypt(firstName);
            }

            if (!string.IsNullOrWhiteSpace(lastName))
            {
                user.LastName = _encryption.Encrypt(lastName);
            }

            if (!string.IsNullOrWhiteSpace(imagePath))
            {
                user.ImagePath = _encryption.Encrypt(imagePath);
            }
        }
        private void DecryptSensitiveFields(User user)
        {
            user.Email = _encryption.Decrypt(user.Email);
            user.FirstName = _encryption.Decrypt(user.FirstName);
            user.LastName = _encryption.Decrypt(user.LastName);
            user.ImagePath = _encryption.Decrypt(user.ImagePath);                
        }

        public IEnumerable<UserDto> GetUsersByRoleId(int roleId)
        {
            var users = _userRepository.GetUsersByRoleId(roleId);
            foreach (var user in users)
            {
                DecryptSensitiveFields(user);
            }

            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public UserDto? GetUserByUsername(string username)
        {
            var user = _userRepository.GetByUsername(username);
            if (user == null)
            {
                throw new ArgumentException($"User not found.");
            }

            DecryptSensitiveFields(user);

            return _mapper.Map<UserDto>(user);
        }
    }
}
