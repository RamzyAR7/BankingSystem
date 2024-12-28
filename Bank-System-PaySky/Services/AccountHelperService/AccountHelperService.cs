using Bank_System_PaySky.Data;
using Bank_System_PaySky.Entities.AccountModels;
using Bank_System_PaySky.Entities.TransactionsModels;
using Bank_System_PaySky.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Bank_System_PaySky.Services.AccountHelper
{
    public class AccountHelperService : IAccountHelperService
    {
        private readonly BankingDbContext _dbContext;

        public AccountHelperService(BankingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Method to get account details by account ID
        public async Task<Account> GetAccountByIdAsync(Guid accountId)
        {
            var account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId)
                ?? throw new AccountNotFoundException($"Account with ID {accountId} is not found.");

            return account;
        }

        // Method to get transaction details by transaction ID
        public async Task<Transaction> GetTransactionByIdAsync(Guid transactionId)
        {
            var transaction = await _dbContext.Transactions
                .Include(t => t.AccountTransactions)
                .FirstOrDefaultAsync(t => t.TransactionId == transactionId)
                ?? throw new TransactionNotFoundException($"Transaction with ID {transactionId} is not found.");

            return transaction;
        }

        // Method to check if an account is a saving account
        public async Task<bool> IsSavingAccountAsync(Guid accountId)
        {
            var account = await GetAccountByIdAsync(accountId);
            return account is SavingAccount;
        }
    }
}
