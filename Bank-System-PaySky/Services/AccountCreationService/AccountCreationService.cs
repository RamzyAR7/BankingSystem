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
                Balance = account.Balance
            };

            if (account is SavingAccount savingAccount)
            {
                accountResponse.Interest = savingAccount.InterestRate;
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
                Interest = a is SavingAccount savingAccount ? savingAccount.InterestRate : null,
                Overdrafts = a is CheckingAccount checkingAccount ? checkingAccount.Overdrafts : null
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

            var newAccount = new CheckingAccount
            {
                AccountNumbers = account.AccountNumbers,
                Balance = account.Balance,
                Overdrafts = account.Overdrafts.Value
            };
            await _dbContext.Accounts.AddAsync(newAccount);
            await _dbContext.SaveChangesAsync();

            return new AccountResponse
            {
                AccountId = newAccount.AccountId,
                AccountNumbers = newAccount.AccountNumbers,
                AccountType = newAccount.GetType().Name,
                Balance = newAccount.Balance,
                Overdrafts = newAccount.Overdrafts
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

            var newAccount = new SavingAccount
            {
                AccountNumbers = account.AccountNumbers,
                Balance = account.Balance,
                InterestRate = account.Interest.Value
            };
            await _dbContext.Accounts.AddAsync(newAccount);
            await _dbContext.SaveChangesAsync();

            return new AccountResponse
            {
                AccountId = newAccount.AccountId,
                AccountNumbers = newAccount.AccountNumbers,
                AccountType = newAccount.GetType().Name,
                Balance = newAccount.Balance,
                Interest = newAccount.InterestRate
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
                Interest = account is SavingAccount savingAccount ? savingAccount.InterestRate : null,
                Overdrafts = account is CheckingAccount checkingAccount ? checkingAccount.Overdrafts : null
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
