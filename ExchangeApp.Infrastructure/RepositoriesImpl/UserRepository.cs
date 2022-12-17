using ExchangeApp.Core.Entities;
using ExchangeApp.Core.RepositoriesInterface;
using ExchangeApp.Helpers;
using ExchangeApp.Infrastructure.Data.Pg.Context;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExchangeApp.Infrastructure.RepositoriesImpl
{
    public class UserRepository : IUserRepository
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly PgDbContext _db;

        public UserRepository(PgDbContext db, SignInManager<ApplicationUser> signInManager)
        {
            _db = db;
            _signInManager = signInManager;
        }

        public async Task<ApplicationUser?> FindByUsernameAsync(string username)
        {
            try
            {
                ApplicationUser? user =
                    await _db.Users
                             .Where(x => x.NormalizedUserName == username.ToUpper())
                             .FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ApplicationUser?> FindByIdAsync(string id)
        {
            try
            {
                ApplicationUser? user =
                    await _db.Users
                             .Where(x => x.Id == id)
                             .FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Result> SignInUserAsync(ApplicationUser user, string password)
        {
            try
            {
                Result resultAuth = await AuthenticationUserByUsernameByPassAsync(user, password);
                if (resultAuth.IsSuccess)
                {
                    Result resultUpdateBlock = await UpdateUserBlockAttemptsFailedAsync(user.Id, false);
                    if (resultUpdateBlock.IsFailed)
                        return resultUpdateBlock;

                    return Result.Ok();
                }

                Result resultIncrement = await UpdateUserBlockAttemptsFailedAsync(user.Id, true);
                if (resultIncrement.IsFailed)
                    return resultIncrement;

                return Result.Fail($"{ConstantsMessagesUser.ErrorCredentialsIncorrect} - Tentativas restantes: {user.AccessFailedCount + 1}");
            }
            catch (Exception ex)
            {
                return Result.Fail(ConstantsMessagesUser.ErrorExceptionAuth + user.UserName);
            }
        }

        public async Task<List<string>> FindRolesByUserAsync(ApplicationUser user)
        {
            try
            {
                var roles =
                    await _signInManager
                            .UserManager
                            .GetRolesAsync(user);

                return roles.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        public async Task<Result> GenerateRefreshTokenAsync(ApplicationUser user)
        {
            try
            {
                string refreshToken =
                    await _signInManager
                                .UserManager
                                .GenerateUserTokenAsync(user, Tokens.LoginProviderRefreshTokenApp, Tokens.PurposeApp);

                await _signInManager
                            .UserManager
                            .SetAuthenticationTokenAsync(user, Tokens.LoginProviderRefreshTokenApp, Tokens.PurposeApp, refreshToken);

                return Result.Ok().WithSuccess(refreshToken);
            }
            catch (Exception ex)
            {
                return Result.Fail(ConstantsMessagesUser.ErrorGenerateTokenRefresh + user.UserName);
            }
        }

        public async Task<string> FindRefreshTokenAuthenticationAsync(ApplicationUser user)
        {
            try
            {
                string refreshToken =
                    await _signInManager
                                .UserManager
                                .GetAuthenticationTokenAsync(user, Tokens.LoginProviderRefreshTokenApp, Tokens.PurposeApp);

                return refreshToken;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<Result> UpdateUserBlockAttemptsFailedAsync(string idUser, bool isIncrement)
        {
            try
            {
                ApplicationUser user = await FindByIdAsync(idUser);
                if (user is null)
                    return Result.Fail(ConstantsMessagesUser.ErrorUserNotFound + idUser);

                if (!isIncrement)
                {
                    user.AccessFailedCount = 0;
                    user.LockoutEnd = null;
                }
                else
                {
                    user.AccessFailedCount += 1;
                    if (user.AccessFailedCount == Tokens.AttemptsLoginFailed)
                    {
                        user.LockoutEnd = DateTime.UtcNow;
                    }
                }
                user.UpdatedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(ConstantsMessagesUser.ErrorUpdateUserBlockAttempts + idUser);
            }
        }

        private async Task<Result> AuthenticationUserByUsernameByPassAsync(ApplicationUser user, string password)
        {
            try
            {
                SignInResult resultAuth =
                    await _signInManager
                                .PasswordSignInAsync(
                                    user,
                                    password,
                                    false,
                                    false
                                );

                if (resultAuth.Succeeded)
                    return Result.Ok();

                return Result.Fail(ConstantsMessagesUser.ErrorCredentialsIncorrect);
            }
            catch (Exception ex)
            {
                return Result.Fail(ConstantsMessagesUser.ErrorExceptionAuth + user.UserName);
            }
        }
    }
}
