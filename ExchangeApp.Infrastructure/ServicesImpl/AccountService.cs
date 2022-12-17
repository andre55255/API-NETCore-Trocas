using AutoMapper;
using ExchangeApp.Communication.ViewObjects.Account;
using ExchangeApp.Communication.ViewObjects.User;
using ExchangeApp.Core.Entities;
using ExchangeApp.Core.RepositoriesInterface;
using ExchangeApp.Core.ServicesInterface;
using ExchangeApp.Helpers;
using FluentResults;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExchangeApp.Infrastructure.ServicesImpl
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AccountService(IUserRepository userRepo, IMapper mapper, IConfiguration configuration)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<TokenUserVO> LoginUserAsync(LoginVO model)
        {
            TokenUserVO response = new TokenUserVO();
            try
            {
                ApplicationUser? user =
                    await _userRepo.FindByUsernameAsync(model.Username);

                if (user is null)
                {
                    response.Error = ConstantsMessagesUser.ErrorUserNotFound + model.Username;
                    return response;
                }

                Result resultValid = ValidateLoginData(user);
                if (resultValid.IsFailed)
                {
                    response.Error = resultValid.Errors.FirstOrDefault().Message;
                    return response;
                }

                Result resultSignIn = await _userRepo.SignInUserAsync(user, model.Password);
                if (resultSignIn.IsFailed)
                {
                    response.Error = resultSignIn.Errors.FirstOrDefault().Message;
                    return response;
                }

                List<string> rolesUser = await _userRepo.FindRolesByUserAsync(user);
                if (rolesUser is null && rolesUser.Count() <= 0)
                {
                    response.Error = ConstantsMessagesUser.ErrorFindRolesByUser + model.Username;
                    return response;
                }

                TokenUserVO resultGenerateToken = await CreateTokenJwt(user, rolesUser);
                if (!string.IsNullOrEmpty(resultGenerateToken.Error))
                {
                    return resultGenerateToken;
                }

                resultGenerateToken.User = _mapper.Map<UserVO>(user);
                resultGenerateToken.User.RolesName = rolesUser;
                return resultGenerateToken;
            }
            catch (Exception ex)
            {
                response.Error = ConstantsMessagesUser.ErrorLogin + model?.Username;
                return response;
            }
        }

        public async Task<TokenUserVO> RefreshTokenUserAsync(RefreshTokenVO model)
        {
            TokenUserVO response = new TokenUserVO();
            try
            {
                string secretAppSettings = _configuration["JWT:Secret"];
                SymmetricSecurityKey securityKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretAppSettings));

                TokenValidationParameters validationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateLifetime = false
                };

                JwtSecurityTokenHandler handlerToken = new JwtSecurityTokenHandler();
                SecurityToken securityToken;
                ClaimsPrincipal principal =
                    handlerToken.ValidateToken(model.AccessToken, validationParameters, out securityToken);

                JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512))
                {
                    response.Error = ConstantsMessagesUser.ErrorInvalidToken + model.AccessToken;
                    return response;
                }

                string username = principal.Identity.Name;
                ApplicationUser user = await _userRepo.FindByUsernameAsync(username);
                if (user is null)
                {
                    response.Error = ConstantsMessagesUser.ErrorUserNotFound + username;
                    return response;
                }

                List<string> rolesUser = await _userRepo.FindRolesByUserAsync(user);
                if (rolesUser is null || rolesUser.Count() <= 0)
                {
                    response.Error = ConstantsMessagesUser.ErrorFindRolesByUser + username;
                    return response;
                }

                string refreshTokenSave = await _userRepo.FindRefreshTokenAuthenticationAsync(user);
                if (string.IsNullOrEmpty(refreshTokenSave))
                {
                    response.Error = ConstantsMessagesUser.ErrorFindTokenRefresh + username;
                    return response;
                }

                if (refreshTokenSave != model.RefreshToken)
                {
                    response.Error = ConstantsMessagesUser.ErrorRefreshTokenIncorrect + username;
                    return response;
                }

                TokenUserVO jwt = await CreateTokenJwt(user, rolesUser);
                return jwt;
            }
            catch (Exception ex)
            {
                response.Error = ConstantsMessagesUser.ErrorRefreshToken;
                return response;
            }
        }

        private async Task<TokenUserVO> CreateTokenJwt(ApplicationUser user, List<string> rolesUser)
        {
            TokenUserVO response = new TokenUserVO();
            try
            {
                List<Claim> claims = GetClaimsListToken(user, rolesUser);
                if (claims is null)
                {
                    response.Error = ConstantsMessagesUser.ErrorBuildClaimsJwt + user.UserName;
                    return response;
                }

                string secretAppSettings = _configuration["JWT:Secret"];
                SymmetricSecurityKey securityKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretAppSettings));

                SigningCredentials credentials =
                    new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

                string issuer = _configuration["JWT:validIssuer"];
                string audience = _configuration["JWT:validAudience"];
                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    expires: DateTime.Now.AddMinutes(Tokens.TimeExpiresTokenJwtMinutes),
                    claims: claims,
                    signingCredentials: credentials
                );

                Result resultGenerateRefresh = await _userRepo.GenerateRefreshTokenAsync(user);
                if (resultGenerateRefresh.IsFailed)
                {
                    response.Error = resultGenerateRefresh.Errors.FirstOrDefault().Message;
                    return response;
                }

                JwtSecurityTokenHandler handlerToken = new JwtSecurityTokenHandler();
                response.AccessToken = handlerToken.WriteToken(token);
                response.Expiration = token.ValidTo.AddHours(-3);
                response.RefreshToken = resultGenerateRefresh.Successes.FirstOrDefault().Message;

                return response;
            }
            catch (Exception ex)
            {
                response.Error = ConstantsMessagesUser.ErrorCreateTokenJwt + user.UserName;
                return response;
            }
        }

        private List<Claim> GetClaimsListToken(ApplicationUser user, List<string> rolesUser)
        {
            try
            {
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                foreach (string role in rolesUser)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                return claims;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private Result ValidateLoginData(ApplicationUser user)
        {
            try
            {
                if (user.AccessFailedCount >= Tokens.AttemptsLoginFailed)
                {
                    return Result.Fail(ConstantsMessagesUser.ErrorAttemptsLoginExceeded);
                }
                if (user.LockoutEnd.HasValue &&
                    user.LockoutEnd > DateTime.Now)
                {
                    return Result.Fail(ConstantsMessagesUser.ErrorAccountBlocked + user.LockoutEnd.Value.ToString("dd/MM/yyyy HH:mm"));
                }
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(ConstantsMessagesUser.ErrorValidateLogin + user.UserName);
            }
        }
    }
}
