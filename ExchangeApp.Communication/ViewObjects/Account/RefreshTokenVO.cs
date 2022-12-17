using System.ComponentModel.DataAnnotations;

namespace ExchangeApp.Communication.ViewObjects.Account
{
    public class RefreshTokenVO
    {
        [Required(ErrorMessage = "Token não informado")]
        public string AccessToken { get; set; }
        [Required(ErrorMessage = "Refresh token não informado")]
        public string RefreshToken { get; set; }
    }
}
