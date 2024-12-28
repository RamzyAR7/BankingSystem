using Bank_System_PaySky.Exceptions;
using Bank_System_PaySky.Models.Accounts;
using Bank_System_PaySky.Services;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace Bank_System_PaySky.Controllers
{
    /// <summary>
    /// Controller for get transactions.
    /// </summary>
    [ApiController]
    [Route("api/transaction")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionsService _transactions;
        private readonly ILogger<TransactionsController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionsController"/> class.
        /// </summary>
        /// <param name="transactions">The transactions service.</param>
        public TransactionsController(ITransactionsService transactions, ILogger<TransactionsController> logger)
        {
            _transactions = transactions;
            _logger = logger;

        }

        /// <summary>
        /// Gets all transactions.
        /// </summary>
        /// <returns>A list of all transactions.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllTransactions()
        {
            var reqId = HttpContext.Items["RequestId"]?.ToString();
            _logger.LogInformation($"(GetAllTransactions) Request ==> {reqId}; Entered ==> {nameof(GetAllTransactions)}");
            var transactions = _transactions.GetAllTransactionsAsync();
            _logger.LogInformation($"(GetAllTransactions Successful) Request ==> {reqId}; Entered ==> {nameof(GetAllTransactions)}");
            return Ok(transactions);
        }

        /// <summary>
        /// Gets a transaction by its ID.
        /// </summary>
        /// <param name="id">The transaction ID.</param>
        /// <returns>The transaction details.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetTransactionById(Guid id)
        {
            var reqId = HttpContext.Items["RequestId"]?.ToString();
            _logger.LogInformation($"(GetTransactionById) Request ==> {reqId}; Entered ==> {nameof(GetTransactionById)}");
            var transaction = _transactions.GetTransactionByIdAsync(id);
            _logger.LogInformation($"(GetTransactionById Successful) Request ==> {reqId}; Entered ==> {nameof(GetTransactionById)}");
            return Ok(transaction);
        }
    }
}
