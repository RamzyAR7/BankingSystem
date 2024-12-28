using Azure.Core;
using Bank_System_PaySky.Exceptions;
using Bank_System_PaySky.Exeptions;
using Bank_System_PaySky.Models.Transactions;
using Bank_System_PaySky.Services;
using Microsoft.AspNetCore.Mvc;


namespace Bank_System_PaySky.Controllers
{
    /// <summary>
    /// Controller for managing account transactions.
    /// </summary>
    [ApiController]
    [Route("api/accounts")]
    public class AccountTransactionsController : ControllerBase
    {
        private readonly IAccountTransactionService _accountService;
        private readonly ILogger<AccountTransactionsController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountTransactionsController"/> class.
        /// </summary>
        /// <param name="accountService">The account transaction service.</param>
        public AccountTransactionsController(IAccountTransactionService accountService, ILogger<AccountTransactionsController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        /// <summary>
        /// Deposits an amount into an account.
        /// </summary>
        /// <param name="request">The deposit request.</param>
        /// <returns>A success message.</returns>
        [HttpPost("deposit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Deposit([FromBody] DepositRequest request)
        {
            var reqId = HttpContext.Items["RequestId"]?.ToString();
            _logger.LogInformation($"(Deposit) Request ==> {reqId}; Entered ==> {nameof(Deposit)}");

            await _accountService.DepositAsync(request.AccountId, request.Amount);

            _logger.LogInformation($"(Deposit Successful) Request ==> {reqId}, Entered ==> {nameof(Deposit)}");
            return Ok(new { Message = "Deposit successful." });
        }

        /// <summary>
        /// Withdraws an amount from an account.
        /// </summary>
        /// <param name="request">The withdrawal request.</param>
        /// <returns>A success message.</returns>
        [HttpPost("withdraw")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Withdraw([FromBody] WithdrawRequest request)
        {
            var reqId = HttpContext.Items["RequestId"]?.ToString();
            _logger.LogInformation($"(Withdraw) Request ==> {reqId}; Entered ==> {nameof(Withdraw)}");

            await _accountService.WithdrawAsync(request.AccountId, request.Amount);

            _logger.LogInformation($"(Withdraw Successful) Request ==> {reqId}; Entered ==> {nameof(Withdraw)}");
            return Ok(new { Message = "Withdrawal successful." });
        }


        /// <summary>
        /// Transfers an amount from one account to another.
        /// </summary>
        /// <param name="request">The transfer request.</param>
        /// <returns>A success message.</returns>
        [HttpPost("transfer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Transfer([FromBody] TransferRequest request)
        {
            var reqId = HttpContext.Items["RequestId"]?.ToString();
            _logger.LogInformation($"(Transfer) Request ==> {reqId}; Entered ==> {nameof(Transfer)}");
            await _accountService.TransferAsync(request.SourceAccountId, request.TargetAccountId, request.Amount);
            _logger.LogInformation($"(Transfer Successful) Request ==> {reqId}; Entered ==> {nameof(Transfer)}");
            return Ok(new { Message = "Transfer successful." });
        }

        /// <summary>
        /// Gets the balance of an account.
        /// </summary>
        /// <param name="accountId">The account ID.</param>
        /// <returns>The account balance.</returns>
        [HttpGet("{accountId}/balance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBalance(Guid accountId)
        {
            var reqId = HttpContext.Items["RequestId"]?.ToString();
            _logger.LogInformation($"(GetBalance) Request ==> {reqId}; Entered ==> {nameof(GetBalance)}");
            var balance = await _accountService.GetBalanceAsync(accountId);
            _logger.LogInformation($"(GetBalance Successful) Request ==> {reqId}; Entered ==> {nameof(GetBalance)}");
            return Ok(new { AccountId = accountId, Balance = balance });
        }


        /// <summary>
        /// Adds interest to an account for a specified number of years.
        /// </summary>
        /// <param name="accountId">The account ID.</param>
        /// <param name="years">The number of years.</param>
        /// <returns>A success message.</returns>
        [HttpPost("addInterest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddInterest([FromQuery] Guid accountId, [FromQuery] int years)
        {
            var reqId = HttpContext.Items["RequestId"]?.ToString();
            _logger.LogInformation($"(AddInterest) Request ==> {reqId}; Entered ==> {nameof(AddInterest)}");
            await _accountService.AddInterestAsync(accountId, years);
            _logger.LogInformation($"(AddInterest Successful) Request ==> {reqId}; Entered ==> {nameof(AddInterest)}");
            return Ok("Interest added successfully.");
        }
    }
}
