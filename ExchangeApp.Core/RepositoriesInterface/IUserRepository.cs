using ExchangeApp.Core.Entities;
using FluentResults;

namespace ExchangeApp.Core.RepositoriesInterface
{
    public interface IUserRepository
    {
        public Task<ApplicationUser?> FindByUsernameAsync(string username);
        public Task<ApplicationUser?> FindByIdAsync(string id);
        public Task<Result> SignInUserAsync(ApplicationUser user, string password);
        public Task<List<string>> FindRolesByUserAsync(ApplicationUser user);
        public Task<Result> GenerateRefreshTokenAsync(ApplicationUser user);
        public Task<string> FindRefreshTokenAuthenticationAsync(ApplicationUser user);
    }
}
