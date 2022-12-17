using ExchangeApp.Communication.ViewObjects.Account;

namespace ExchangeApp.Core.ServicesInterface
{
    public interface IAccountService
    {
        public Task<TokenUserVO> LoginUserAsync(LoginVO model);
        public Task<TokenUserVO> RefreshTokenUserAsync(RefreshTokenVO model);
    }
}
