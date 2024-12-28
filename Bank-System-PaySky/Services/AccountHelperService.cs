using Bank_System_PaySky.Data;
using Bank_System_PaySky.Entites.AccountModdels;
using Bank_System_PaySky.Entities.AccountModdels;
using Bank_System_PaySky.Entities.TransactionsModels;
using Bank_System_PaySky.Exceptions;
using Bank_System_PaySky.Exeptions;
using Microsoft.EntityFrameworkCore;

namespace Bank_System_PaySky.Services
{
    public class AccountHelperService : IAccountHelperService
    {
        public readonly BankingDbContext _dbContext;

        public AccountHelperService(BankingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Account> GetAccountByIdAsync(Guid accountId)
        {
            var account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId)
                ?? throw new AccountNotFoundException($"Account with ID {accountId} is not found.");

            return account;
        }

        public async Task<Transaction> GetTransactionByIdAsync(Guid transactionId)
        {
            var transaction = await _dbContext.Transactions
                .Include(t => t.AccountTransactions)
                .FirstOrDefaultAsync(t => t.TransactionId == transactionId)
                ?? throw new TransactionNotFounfException($"Transaction with ID {transactionId} is not found.");

            return transaction;
        }

        public async Task<bool> IsSavingAccountAsync(Guid accountId)
        {
            var account = await GetAccountByIdAsync(accountId);
            return account is SavingAccount;
        }
    }
}
