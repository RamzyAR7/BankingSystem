using Bank_System_PaySky.Auth;
using Bank_System_PaySky.Dtos.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank_System_PaySky.Controllers
{
    /// <summary>
    /// Controller for managing user authentication and token generation.
    /// </summary>
 
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserLoginService _userLoginService;
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="logger"> The logger instance.</param>
        /// <param name="userLoginService"> The user login service instance.</param>
        public AuthController(IUserLoginService userLoginService, ILogger<AuthController> logger)
        {
            _userLoginService = userLoginService;
            _logger = logger;
        }
        /// <summary>
        /// Logs in a user and generates a token.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>fullToken</returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TokenResDto>> Login(UserLoginDto request)
        {
            var fullToken = await _userLoginService.LoginAsync(request);
            _logger.LogInformation($"(Login) Request ==> {HttpContext.Items["RequestId"]?.ToString()}; Entered ==> {nameof(Login)}");
            if (fullToken == null)
            {
                return BadRequest("username or password is wrong");
            }
            return Ok(fullToken);
        }
        /// <summary>
        /// Refreshes the token for a user.
        /// </summary>
        /// param name="request"></param>
        /// returns>res</returns>
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<TokenResDto> RefreshToken(RefreshTokenReqDto request)
        {
            var res = _userLoginService.RefreshToken(request);
            _logger.LogInformation($"(RefreshToken) Request ==> {HttpContext.Items["RequestId"]?.ToString()}; Entered ==> {nameof(RefreshToken)}");
            if (res == null || res.AccessToken is null || res.RefreshToken is null)
            {
                return Unauthorized("Invaild refresh token");
            }
            return Ok(res);
        }
        
    }
}
