using Bank_System_PaySky.Data;
using Bank_System_PaySky.Models.Transactions;
using Bank_System_PaySky.Models.Accounts;
using Microsoft.EntityFrameworkCore;
using Bank_System_PaySky.Exceptions;
using System.Security.Principal;
using Bank_System_PaySky.Services.AccountHelper;
using Bank_System_PaySky.Entities.AccountModels;

namespace Bank_System_PaySky.Services.AccountCreation
{
    public class AccountCreationService : IAccountCreationService
    {
        private readonly BankingDbContext _dbContext;
        private readonly IAccountHelperService _accountHelper;

        public AccountCreationService(BankingDbContext dbContext, IAccountHelperService accountHelper)
        {
            _dbContext = dbContext;
            _accountHelper = accountHelper;
        }

        // Method to get account details by account ID
        public async Task<AccountResponse> GetAccountByIdAsync(Guid accountId)
        {
            Account account = await _accountHelper.GetAccountByIdAsync(accountId);

            var accountResponse = new AccountResponse
            {
                AccountId = account.AccountId,
                AccountNumbers = account.AccountNumbers,
                AccountType = account.GetType().Name,
                Balance = account.Balance,
                CurrencyCode = account.CurrencyCode,
                UserId = account.UserId
            };

            if (account is SavingAccount savingAccount)
            {
                accountResponse.Interest = savingAccount.Interest;
            }
            else if (account is CheckingAccount checkingAccount)
            {
                accountResponse.Overdrafts = checkingAccount.Overdrafts;
            }

            return accountResponse;
        }

        // Method to get all accounts
        public async Task<IEnumerable<AccountResponse>> GetAllAccountsAsync()
        {
            var accounts = await _dbContext.Accounts.ToListAsync();
            return accounts.Select(a => new AccountResponse
            {
                AccountId = a.AccountId,
                AccountNumbers = a.AccountNumbers,
                AccountType = a.GetType().Name,
                Balance = a.Balance,
                Interest = a is SavingAccount savingAccount ? savingAccount.Interest : null,
                Overdrafts = a is CheckingAccount checkingAccount ? checkingAccount.Overdrafts : null,
                CurrencyCode = a.CurrencyCode,
                UserId = a.UserId
            }).ToList();
        }

        // Method to create a new checking account
        public async Task<AccountResponse> CreateCheckingAccountAsync(CreateCheckingAccountRequest account)
        {
            bool isAccountExist = await _dbContext.Accounts.AnyAsync(a => a.AccountNumbers == account.AccountNumbers);

            if (isAccountExist)
            {
                throw new InvalidAccountNumberException($"Account with number {account.AccountNumbers} already exists.");
            }
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == account.UserId);
            if (user == null)
            {
                throw new InvalidAccountNumberException($"User with ID {account.UserId} does not exist.");
            }
            var currency = await _dbContext.Currencies.FirstOrDefaultAsync(c => c.CurrencyCode == account.CurrencyCode);
            if (currency == null)
            {
                throw new InvalidAccountNumberException($"Currency with code {account.CurrencyCode} does not exist."); // MUST BE HANDLE
            }
            var newAccount = new CheckingAccount
            {
                AccountNumbers = account.AccountNumbers,
                Balance = account.Balance,
                Overdrafts = account.Overdrafts.Value,
                CurrencyCode = account.CurrencyCode,
                UserId = account.UserId
            };
            await _dbContext.Accounts.AddAsync(newAccount);
            await _dbContext.SaveChangesAsync();

            return new AccountResponse
            {
                AccountId = newAccount.AccountId,
                AccountNumbers = newAccount.AccountNumbers,
                AccountType = newAccount.GetType().Name,
                Balance = newAccount.Balance,
                Overdrafts = newAccount.Overdrafts,
                CurrencyCode = newAccount.CurrencyCode,
                UserId = account.UserId
            };
        }

        // Method to create a new saving account
        public async Task<AccountResponse> CreateSavingAccountAsync(CreateSavingAccountRequest account)
        {
            bool isAccountExist = await _dbContext.Accounts.AnyAsync(a => a.AccountNumbers == account.AccountNumbers);

            if (isAccountExist)
            {
                throw new InvalidAccountNumberException($"Account with number {account.AccountNumbers} already exists.");
            }
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == account.UserId);
            if (user == null)
            {
                throw new InvalidAccountNumberException($"User with ID {account.UserId} does not exist.");
            }
            var currency = await _dbContext.Currencies.FirstOrDefaultAsync(c => c.CurrencyCode == account.CurrencyCode);
            if (currency == null)
            {
                throw new InvalidAccountNumberException($"Currency with code {account.CurrencyCode} does not exist.");
            }

            var newAccount = new SavingAccount
            {
                AccountNumbers = account.AccountNumbers,
                Balance = account.Balance,
                Interest = account.Interest,
                CurrencyCode = account.CurrencyCode,
                UserId = account.UserId
            };
            await _dbContext.Accounts.AddAsync(newAccount);
            await _dbContext.SaveChangesAsync();

            return new AccountResponse
            {
                AccountId = newAccount.AccountId,
                AccountNumbers = newAccount.AccountNumbers,
                AccountType = newAccount.GetType().Name,
                Balance = newAccount.Balance,
                Interest = newAccount.Interest,
                CurrencyCode = newAccount.CurrencyCode,
                UserId = account.UserId
            };
        }

        // Method to update an existing account
        public async Task<AccountResponse> UpdateAccountAsync(Guid accountId, UpdateAccountRequest updatedAccount)
        {
            var account = await _accountHelper.GetAccountByIdAsync(accountId);
            account.Balance = updatedAccount.Balance;
            await _dbContext.SaveChangesAsync();
            return new AccountResponse
            {
                AccountId = account.AccountId,
                AccountNumbers = account.AccountNumbers,
                AccountType = account.GetType().Name,
                Balance = account.Balance,
                Interest = account is SavingAccount savingAccount ? savingAccount.Interest : null,
                Overdrafts = account is CheckingAccount checkingAccount ? checkingAccount.Overdrafts : null,
                CurrencyCode = account.CurrencyCode,
                UserId = account.UserId
            };
        }

        // Method to delete an account
        public async Task DeleteAccountAsync(Guid accountId)
        {
            var account = await _accountHelper.GetAccountByIdAsync(accountId);

            _dbContext.Accounts.Remove(account);
            await _dbContext.SaveChangesAsync();
        }
    }
}
