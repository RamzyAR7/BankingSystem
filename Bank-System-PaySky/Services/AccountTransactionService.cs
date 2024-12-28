using Bank_System_PaySky.Data;
using Bank_System_PaySky.Entites.AccountModdels;
using Bank_System_PaySky.Entities.AccountModdels;
using Bank_System_PaySky.Entities.AccountTransactionsModels;
using Bank_System_PaySky.Entities.TransactionsModels;
using Bank_System_PaySky.Exceptions;


namespace Bank_System_PaySky.Services
{
    public class AccountTransactionService : IAccountTransactionService
    {
        private readonly BankingDbContext _dbContext;
        private readonly IAccountHelperService _accountHelper;
        public AccountTransactionService(BankingDbContext dbContext, IAccountHelperService accountHelper)
        {
            _dbContext = dbContext;
            _accountHelper = accountHelper;
        }

        public async Task<decimal> GetBalanceAsync(Guid accountId)
        {
            var account = await _accountHelper.GetAccountByIdAsync(accountId);
            return account.Balance;
        }

        public async Task DepositAsync(Guid accountId, decimal amount)
        {
            Account account = await _accountHelper.GetAccountByIdAsync(accountId);
            account.Deposit(amount);

            var transaction = new Transaction()
            {
                Amount = amount,
                Type = TransactionType.Deposit.ToString(),
                Timestamp = DateTime.UtcNow
            };
            await _dbContext.Transactions.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();

            var accountTransaction = new AccountTransactions()
            {
                AccountId = accountId,
                TransactionId = transaction.TransactionId,
                AccountStatus = "Source"
            };

            await _dbContext.AccountTransactions.AddAsync(accountTransaction);
            await _dbContext.SaveChangesAsync();
        }

        public async Task WithdrawAsync(Guid accountId, decimal amount)
        {
            Account account = await _accountHelper.GetAccountByIdAsync(accountId);
            account.WithDraw(amount);

            var transaction = new Transaction()
            {
                Amount = amount,
                Type = TransactionType.Withdraw.ToString(),
                Timestamp = DateTime.UtcNow
            };
            await _dbContext.Transactions.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();

            var accountTransaction = new AccountTransactions()
            {
                AccountId = accountId,
                TransactionId = transaction.TransactionId,
                AccountStatus = "Source"
            };
            await _dbContext.AccountTransactions.AddAsync(accountTransaction);
            await _dbContext.SaveChangesAsync();
        }

        public async Task TransferAsync(Guid sourceAccountId, Guid targetAccountId, decimal amount)
        {
            Account sourceAccount = await _accountHelper.GetAccountByIdAsync(sourceAccountId);
            Account targetAccount = await _accountHelper.GetAccountByIdAsync(targetAccountId);

            sourceAccount.Transfer(targetAccount, amount);

            var transaction = new Transaction()
            {
                Amount = amount,
                Type = TransactionType.Transfer.ToString(),
                Timestamp = DateTime.UtcNow
            };
            await _dbContext.Transactions.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();

            var sourceAccountTransaction = new AccountTransactions()
            {
                AccountId = sourceAccount.AccountId,
                TransactionId = transaction.TransactionId,
                AccountStatus = "Source"
            };

            var targetAccountTransaction = new AccountTransactions()
            {
                AccountId = targetAccount.AccountId,
                TransactionId = transaction.TransactionId,
                AccountStatus = "Target"
            };

            await _dbContext.AccountTransactions.AddRangeAsync(sourceAccountTransaction, targetAccountTransaction);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddInterestAsync(Guid accountId, int years)
        {
            if (!await _accountHelper.IsSavingAccountAsync(accountId))
            {
                throw new InvalidAccountOperationException("The specified account is not a saving account.");
            }
            var savingAccount = (SavingAccount)await _accountHelper.GetAccountByIdAsync(accountId);
            savingAccount.AddInterest(years);
            await _dbContext.SaveChangesAsync();

        }
    }
}