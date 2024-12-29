using Bank_System_PaySky.Dtos.Users;
using Bank_System_PaySky.Exceptions;
using Bank_System_PaySky.Services.UserCreationService;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Swashbuckle.AspNetCore.Annotations;

namespace Bank_System_PaySky.Controllers
{
    /// <summary>
    /// Controller for managing user creation, updates, and deletion.
    /// </summary>
    [ApiController]
    [Route("api/users")]
    [ApiExplorerSettings(GroupName = "Users")]
    public class UserCreationController : ControllerBase
    {
        private readonly IUserCreationService _userCreationService;
        private readonly ILogger<UserCreationController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserCreationController"/> class.
        /// </summary>
        /// <param name="userCreationService">The user creation service.</param>
        /// <param name="logger">The logger instance.</param>
        public UserCreationController(IUserCreationService userCreationService, ILogger<UserCreationController> logger)
        {
            _userCreationService = userCreationService;
            _logger = logger;
        }

        /// <summary>
        /// Gets a user by their ID.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>The user details.</returns>
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserAsync(Guid userId)
        {
            var reqId = HttpContext.Items["RequestId"]?.ToString();
            _logger.LogInformation($"(GetUserAsync) Request ==> {reqId}; Entered ==> {nameof(GetUserAsync)}");
            var user = await _userCreationService.GetUserAsync(userId);
            _logger.LogInformation($"(GetUserAsync Successful) Request ==> {reqId}; Entered ==> {nameof(GetUserAsync)}");
            return Ok(user);
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>A list of all users.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsersAsync()
        {
            var reqId = HttpContext.Items["RequestId"]?.ToString();
            _logger.LogInformation($"(GetUsersAsync) Request ==> {reqId}; Entered ==> {nameof(GetUsersAsync)}");
            var users = await _userCreationService.GetUsersAsync();
            _logger.LogInformation($"(GetUsersAsync Successful) Request ==> {reqId}; Entered ==> {nameof(GetUsersAsync)}");
            return Ok(users);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="userRequest">The user creation request.</param>
        /// <returns>The created user details.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequest userRequest)
        {
            var reqId = HttpContext.Items["RequestId"]?.ToString();
            _logger.LogInformation($"(CreateUserAsync) Request ==> {reqId}; Entered ==> {nameof(CreateUserAsync)}");
            var createdUser = await _userCreationService.CreateUserAsync(userRequest);
            _logger.LogInformation($"(CreateUserAsync Successful) Request ==> {reqId}; Entered ==> {nameof(CreateUserAsync)}");
            return Ok(createdUser);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="userRequest">The update user request.</param>
        /// <returns>The updated user details.</returns>
        [HttpPut("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserAsync(Guid userId, [FromBody] UpdateUserRequest userRequest)
        {
            var reqId = HttpContext.Items["RequestId"]?.ToString();
            _logger.LogInformation($"(UpdateUserAsync) Request ==> {reqId}; Entered ==> {nameof(UpdateUserAsync)}");
            var updatedUser = await _userCreationService.UpdateUserAsync(userId, userRequest);
            _logger.LogInformation($"(UpdateUserAsync Successful) Request ==> {reqId}; Entered ==> {nameof(UpdateUserAsync)}");
            return Ok(updatedUser);
        }

        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>A success message.</returns>
        [HttpDelete("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUserAsync(Guid userId)
        {
            var reqId = HttpContext.Items["RequestId"]?.ToString();
            _logger.LogInformation($"(DeleteUserAsync) Request ==> {reqId}; Entered ==> {nameof(DeleteUserAsync)}");
            await _userCreationService.DeleteUserAsync(userId);
            _logger.LogInformation($"(DeleteUserAsync Successful) Request ==> {reqId}; Entered ==> {nameof(DeleteUserAsync)}");
            return Ok(new { Message = "User deleted successfully." });
        }
    }
}
