using Bank_System_PaySky.Data;
using Bank_System_PaySky.Exceptions;
using Bank_System_PaySky.Models.Accounts;
using Bank_System_PaySky.Services.AccountHelper;
using Microsoft.EntityFrameworkCore;

namespace Bank_System_PaySky.Services.Transactions
{
    public class TransactionsService : ITransactionsService
    {
        private readonly BankingDbContext _dbContext;
        private readonly IAccountHelperService _helper;

        public TransactionsService(BankingDbContext dbContext, IAccountHelperService helper)
        {
            _helper = helper;
            _dbContext = dbContext;
        }

        // Method to get all transactions
        public async Task<IEnumerable<TransactionResponse>> GetAllTransactionsAsync()
        {
            var transactions = await _dbContext.Transactions
                .Include(t => t.AccountTransactions)
                .Select(t => new TransactionResponse
                {
                    TransactionId = t.TransactionId,
                    SourceAccountId = t.AccountTransactions
                        .Where(at => at.AccountStatus == "Source")
                        .Select(at => at.AccountId)
                        .DefaultIfEmpty()
                        .FirstOrDefault(),
                    TargetAccountId = t.AccountTransactions
                        .Where(at => at.AccountStatus == "Target")
                        .Select(at => at.AccountId)
                        .DefaultIfEmpty()
                        .FirstOrDefault(),
                    SourceCurrancyType = t.AccountTransactions
                        .Where(at => at.AccountStatus == "Source")
                        .Select(at => at.CurrencyCode)
                        .DefaultIfEmpty()
                        .FirstOrDefault(),
                    TargetCurrancyType = t.AccountTransactions
                        .Where(at => at.AccountStatus == "Target")
                        .Select(at => at.CurrencyCode)
                        .DefaultIfEmpty()
                        .FirstOrDefault(),
                    TypeOfOperation = t.TransactionType,
                    AmountToTarget = t.Amount,
                    Timestamp = t.Timestamp
                }).ToListAsync();

            return transactions;
        }

        // Method to get transaction details by transaction ID
        public async Task<TransactionResponse> GetTransactionByIdAsync(Guid transactionId)
        {
            var transaction = await _helper.GetTransactionByIdAsync(transactionId);

            if (transaction == null)
            {
                throw new TransactionNotFoundException($"Transaction with ID {transactionId} not found.");
            }

            if (transaction.AccountTransactions == null)
            {
                throw new AccountNotFoundException("Account transactions are null.");
            }

            var transactionResponse = new TransactionResponse
            {
                TransactionId = transaction.TransactionId,
                SourceAccountId = transaction.AccountTransactions
                    .Where(at => at.AccountStatus == "Source")
                    .Select(at => at.AccountId)
                    .DefaultIfEmpty()
                    .FirstOrDefault(),
                TargetAccountId = transaction.AccountTransactions
                    .Where(at => at.AccountStatus == "Target")
                    .Select(at => at.AccountId)
                    .DefaultIfEmpty()
                    .FirstOrDefault(),
                SourceCurrancyType = transaction.AccountTransactions
                    .Where(at => at.AccountStatus == "Source")
                    .Select(at => at.CurrencyCode)
                    .DefaultIfEmpty()
                    .FirstOrDefault(),
                TargetCurrancyType = transaction.AccountTransactions
                    .Where(at => at.AccountStatus == "Target")
                    .Select(at => at.CurrencyCode)
                    .DefaultIfEmpty()
                    .FirstOrDefault(),
                TypeOfOperation = transaction.TransactionType,
                AmountToTarget = transaction.Amount,
                Timestamp = transaction.Timestamp
            };
            return transactionResponse;
        }
    }
}
