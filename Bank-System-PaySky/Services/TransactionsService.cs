using Bank_System_PaySky.Data;
using Bank_System_PaySky.Exceptions;
using Bank_System_PaySky.Exeptions;
using Bank_System_PaySky.Models.Accounts;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace Bank_System_PaySky.Services
{
    public class TransactionsService:ITransactionsService
    {
        private readonly BankingDbContext _dbContext;
        private readonly IAccountHelperService _helper;

        public TransactionsService(BankingDbContext dbContext, IAccountHelperService helper)
        {
            _helper = helper;
            _dbContext = dbContext;
        }
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
                    TypeOfOperation = t.Type,
                    Amount = t.Amount,
                    Timestamp = t.Timestamp
                }).ToListAsync();

            return transactions;
        }

        public async Task<TransactionResponse> GetTransactionByIdAsync(Guid transactionId)
        {
            var transaction = await _helper.GetTransactionByIdAsync(transactionId);

            if (transaction == null)
            {
                throw new TransactionNotFounfException($"Transaction with ID {transactionId} not found.");
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
                TypeOfOperation = transaction.Type,
                Amount = transaction.Amount,
                Timestamp = transaction.Timestamp
            };

            return transactionResponse;
        }

    }
}
