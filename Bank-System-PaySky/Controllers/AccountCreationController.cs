using Bank_System_PaySky.Entities.AccountModels;
using Bank_System_PaySky.Exceptions;
using Bank_System_PaySky.Models;
using Microsoft.AspNetCore.Mvc;
using Bank_System_PaySky.Models.Accounts;
using Bank_System_PaySky.Dtos.Accounts;
using System.Security.Principal;
using Bank_System_PaySky.Services.AccountCreation;

namespace Bank_System_PaySky.Controllers
{
    /// <summary>
    /// Controller for managing account get, creation, updates, delete.
    /// </summary>
    [ApiController]
    [Route("api/accounts")]
    [ApiExplorerSettings(GroupName = "Accounts")]
    public class AccountCreationController : ControllerBase
    {
        private readonly IAccountCreationService _accountCreationService;
        private readonly ILogger<AccountCreationController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountCreationController"/> class.
        /// </summary>
        /// <param name="accountCreationService">The account creation service.</param>
        /// <param name="logger">The logger instance.</param>
        public AccountCreationController(IAccountCreationService accountCreationService, ILogger<AccountCreationController> logger)
        {
            _accountCreationService = accountCreationService;
            _logger = logger;
        }

        /// <summary>
        /// Gets an account by its ID.
        /// </summary>
        /// <param name="id">The account ID.</param>
        /// <returns>The account details.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAccountById(Guid id)
        {
            var reqId = HttpContext.Items["RequestId"]?.ToString();
            _logger.LogInformation($"(GetAccountById) Request ==> {reqId}; Entered ==> {nameof(GetAccountById)}");
            var account = await _accountCreationService.GetAccountByIdAsync(id);
            _logger.LogInformation($"(GetAccountById Successful) Request ==> {reqId}; Entered ==> {nameof(GetAccountById)}");
            return Ok(account);
        }

        /// <summary>
        /// Gets all accounts.
        /// </summary>
        /// <returns>A list of all accounts.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAccounts()
        {
            var reqId = HttpContext.Items["RequestId"]?.ToString();
            _logger.LogInformation($"(GetAllAccounts) Request ==> {reqId}; Entered ==> {nameof(GetAllAccounts)}");
            var accounts = await _accountCreationService.GetAllAccountsAsync();
            _logger.LogInformation($"(GetAllAccounts Successful) Request ==> {reqId}; Entered ==> {nameof(GetAllAccounts)}");
            return Ok(accounts);
        }

        /// <summary>
        /// Creates a new checking account.
        /// </summary>
        /// <param name="account">The checking account request.</param>
        /// <returns>The created account details.</returns>
        [HttpPost("checking")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCheckingAccount([FromBody] CreateCheckingAccountRequest account)
        {
            var reqId = HttpContext.Items["RequestId"]?.ToString();
            _logger.LogInformation($"(CreateCheckingAccount) Request ==> {reqId}; Entered ==> {nameof(CreateCheckingAccount)}");
            var createdAccount = await _accountCreationService.CreateCheckingAccountAsync(account);
            _logger.LogInformation($"(CreateCheckingAccount Successful) Request ==> {reqId}; Entered ==> {nameof(CreateCheckingAccount)}");
            return CreatedAtAction(nameof(GetAccountById), new { id = createdAccount.AccountId }, createdAccount);
        }

        /// <summary>
        /// Creates a new saving account.
        /// </summary>
        /// <param name="account">The saving account request.</param>
        /// <returns>The created account details.</returns>
        [HttpPost("saving")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSavingAccount([FromBody] CreateSavingAccountRequest account)
        {
            var reqId = HttpContext.Items["RequestId"]?.ToString();
            _logger.LogInformation($"(CreateSavingAccount) Request ==> {reqId}; Entered ==> {nameof(CreateSavingAccount)}");
            var createdAccount = await _accountCreationService.CreateSavingAccountAsync(account);
            _logger.LogInformation($"(CreateSavingAccount Successful) Request ==> {reqId}; Entered ==> {nameof(CreateSavingAccount)}");
            return CreatedAtAction(nameof(GetAccountById), new { id = createdAccount.AccountId }, createdAccount);
        }

        /// <summary>
        /// Updates an existing account.
        /// </summary>
        /// <param name="account">The update account request.</param>
        /// <param name="id">The account ID.</param>
        /// <returns>The updated account details.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountRequest account, Guid id)
        {
            var reqId = HttpContext.Items["RequestId"]?.ToString();
            _logger.LogInformation($"(UpdateAccount) Request ==> {reqId}; Entered ==> {nameof(UpdateAccount)}");
            var updatedAccount = await _accountCreationService.UpdateAccountAsync(id, account);
            _logger.LogInformation($"(UpdateAccount Successful) Request ==> {reqId}; Entered ==> {nameof(UpdateAccount)}");
            return Ok(updatedAccount);
        }

        /// <summary>
        /// Deletes an account by its ID.
        /// </summary>
        /// <param name="id">The account ID.</param>
        /// <returns>A success message.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            var reqId = HttpContext.Items["RequestId"]?.ToString();
            _logger.LogInformation($"(DeleteAccount) Request ==> {reqId}; Entered ==> {nameof(DeleteAccount)}");
            await _accountCreationService.DeleteAccountAsync(id);
            _logger.LogInformation($"(DeleteAccount Successful) Request ==> {reqId}; Entered ==> {nameof(DeleteAccount)}");
            return Ok(new { Message = "Account deleted successfully." });
        }
    }
}
