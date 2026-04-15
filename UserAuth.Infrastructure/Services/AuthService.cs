using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UserAuth.Application.DTOs;
using UserAuth.Application.Interfaces;
using UserAuth.Domain.Entities;

namespace UserAuth.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repo;
        private readonly JwtService _jwt;

        public AuthService(IUserRepository repo, JwtService jwt)
        {
            _repo = repo;
            _jwt = jwt;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            var existing = await _repo.GetByEmailAsync(dto.Email);
            if (existing != null)
                throw new Exception("User already exists");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                Mobile = dto.Mobile,
                PasswordHash = Hash(dto.Password)
            };

            await _repo.AddAsync(user);
            return _jwt.GenerateToken(user);
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _repo.GetByEmailAsync(dto.Email);
            if (user == null || user.PasswordHash != Hash(dto.Password))
                throw new Exception("Invalid credentials");

            return _jwt.GenerateToken(user);
        }

        public async Task<bool> ChangePasswordAsync(string email, ChangePasswordDto dto)
        {
            var user = await _repo.GetByEmailAsync(email);
            if (user == null || user.PasswordHash != Hash(dto.OldPassword))
                return false;

            user.PasswordHash = Hash(dto.NewPassword);
            await _repo.UpdateAsync(user);

            return true;
        }

        private string Hash(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
