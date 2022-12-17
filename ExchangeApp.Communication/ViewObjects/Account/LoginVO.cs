using ExchangeApp.Communication.ViewObjects.User;
using System.ComponentModel.DataAnnotations;

namespace ExchangeApp.Communication.ViewObjects.Account
{
    public class LoginVO
    {
        [Required(ErrorMessage = "Nome de usuário não informado")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Senha não informada")]
        public string Password { get; set; }
    }

    public class TokenUserVO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? Expiration { get; set; }
        public string Error { get; set; }
        public UserVO User { get; set; }
    }
}
