using ExchangeApp.Communication.ViewObjects.Account;
using ExchangeApp.Communication.ViewObjects.Utils;
using ExchangeApp.Core.ServicesInterface;
using ExchangeApp.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accService;

        public AccountController(IAccountService accService)
        {
            _accService = accService;
        }

        /// <summary>
        /// SignIn - Método para realizar login de usuário, passar dados no body
        /// </summary>
        [HttpPost]
        [Route("SignIn")]
        public async Task<IActionResult> SignInAsync([FromBody] LoginVO model)
        {
            APIResponseVO response = new APIResponseVO();
            try
            {
                TokenUserVO token = await _accService.LoginUserAsync(model);
                if (!string.IsNullOrEmpty(token.Error))
                {
                    response.Success = false;
                    response.Message = token.Error;

                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }
                response.Success = true;
                response.Message = ConstantsMessagesUser.SuccessLogin + model.Username;
                response.Object = token;

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ConstantsMessagesUser.ErrorLogin + model.Username;

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Refresh - Método para realizar refresh token, passar dados no body
        /// </summary>
        [HttpPost]
        [Route("Refresh")]
        public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenVO model)
        {
            APIResponseVO response = new APIResponseVO();
            try
            {
                TokenUserVO token = await _accService.RefreshTokenUserAsync(model);
                if (!string.IsNullOrEmpty(token.Error))
                {
                    response.Success = false;
                    response.Message = token.Error;

                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }
                response.Success = true;
                response.Message = ConstantsMessagesUser.SuccessRefreshToken;
                response.Object = token;

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ConstantsMessagesUser.ErrorRefreshToken;

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
