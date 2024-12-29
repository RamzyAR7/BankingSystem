using Bank_System_PaySky.Data;
using Bank_System_PaySky.Entities.AccountModels;
using Bank_System_PaySky.Entities.TransactionsModels;
using Bank_System_PaySky.Entities.AccountTransactionsModels;
using Bank_System_PaySky.Exceptions;
using Bank_System_PaySky.Services.AccountHelper;
using Microsoft.EntityFrameworkCore;


namespace Bank_System_PaySky.Services.AccountTransactionService
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
                TransactionType = TransactionType.Deposit.ToString(),
                Timestamp = DateTime.UtcNow
            };
            await _dbContext.Transactions.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();

            var accountTransaction = new AccountTransactions
            {
                AccountId = accountId,
                TransactionId = transaction.TransactionId,
                TransactionCurrancy = account.CurrencyCode,
                AccountStatus = "Source"
            };

            await _dbContext.AccountTransactions.AddAsync(accountTransaction);
            await _dbContext.SaveChangesAsync();
        }

        public async Task WithdrawAsync(Guid accountId, decimal amount)
        {
            Account account = await _accountHelper.GetAccountByIdAsync(accountId);

            account.Withdraw(amount);

            var transaction = new Transaction()
            {
                Amount = amount,
                TransactionType = TransactionType.Withdraw.ToString(),
                Timestamp = DateTime.UtcNow
            };
            await _dbContext.Transactions.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();

            var accountTransaction = new AccountTransactions
            {
                AccountId = accountId,
                TransactionId = transaction.TransactionId,
                TransactionCurrancy = account.CurrencyCode,
                AccountStatus = "Source"
            };
            await _dbContext.AccountTransactions.AddAsync(accountTransaction);
            await _dbContext.SaveChangesAsync();
        }

        public async Task TransferAsync(Guid sourceAccountId, Guid targetAccountId, decimal amount)
        {
            if (amount <= 0)
            {
                throw new InvalidAccountOperationException("Amount must be greater than zero.");
            }

            // Fetch accounts and validate
            Account sourceAccount = await _accountHelper.GetAccountByIdAsync(sourceAccountId);
            Account targetAccount = await _accountHelper.GetAccountByIdAsync(targetAccountId);

            if (sourceAccount == null)
            {
                throw new AccountNotFoundException("Source account not found.");
            }
            if (targetAccount == null)
            {
                throw new AccountNotFoundException("Target account not found.");
            }

            decimal convertedAmount = amount;

            // Convert amount if currencies differ
            if (sourceAccount.CurrencyCode != targetAccount.CurrencyCode)
            {
                convertedAmount = await _accountHelper.ConvertAsync(sourceAccount.CurrencyCode, targetAccount.CurrencyCode, amount);
            }

            // Validate source account balance
            if (!sourceAccount.IsTransferValid(amount))
            {
                throw new InvalidAccountOperationException("Insufficient funds for the transfer.");
            }

            // Begin transaction
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                // Perform the transfer
                sourceAccount.Withdraw(amount);
                targetAccount.Deposit(convertedAmount);

                // Save account changes
                _dbContext.Accounts.Update(sourceAccount);
                _dbContext.Accounts.Update(targetAccount);

                // Create transaction record
                var transactionRecord = new Transaction
                {
                    Amount = convertedAmount,
                    TransactionType = TransactionType.Transfer.ToString(),
                    Timestamp = DateTime.UtcNow
                };
                await _dbContext.Transactions.AddAsync(transactionRecord);

                // Create AccountTransactions
                var sourceAccountTransaction = new AccountTransactions
                {
                    AccountId = sourceAccount.AccountId,
                    TransactionId = transactionRecord.TransactionId,
                    TransactionCurrancy = sourceAccount.CurrencyCode,
                    AccountStatus = "Source"
                };

                var targetAccountTransaction = new AccountTransactions
                {
                    AccountId = targetAccount.AccountId,
                    TransactionId = transactionRecord.TransactionId,
                    TransactionCurrancy = targetAccount.CurrencyCode,
                    AccountStatus = "Target"
                };

                await _dbContext.AccountTransactions.AddRangeAsync(sourceAccountTransaction, targetAccountTransaction);

                // Commit all changes
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                // Rollback on failure
                await transaction.RollbackAsync();
                throw;
            }
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