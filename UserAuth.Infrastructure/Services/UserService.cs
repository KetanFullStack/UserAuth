using UserAuth.Domain.Entities;
using UserAuth.Application.Interfaces;

namespace UserAuth.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _repo.GetAllAsync();
        }
    }
}
