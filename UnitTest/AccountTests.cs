using Bank_System_PaySky.Data;
using Bank_System_PaySky.Entities.AccountModels;
using Bank_System_PaySky.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Bank_System_PaySky.Models.Accounts;
using Bank_System_PaySky.Services.AccountCreation;
using Bank_System_PaySky.Services.AccountHelper;
using Bank_System_PaySky.Services.Transactions;
using NUnit.Framework;
using Bank_System_PaySky.Services.AccountTransactionService;

namespace UnitTest
{
    [TestFixture]
    public class AccountTests : IDisposable
    {
        private DbContextOptions<BankingDbContext> _dbOptions;
        private BankingDbContext _dbContext;
        private IAccountHelperService _accountHelper;
        private IAccountTransactionService _accountTransactionService;
        private IAccountCreationService _accountCreationService;
        private ITransactionsService _transactionsService;
        private string connectionString;
        private IConfiguration configuration;

        [SetUp]
        public void Setup()
        {
            configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            // Get connection string for the test database
            connectionString = configuration.GetConnectionString("TestDB") ?? throw new InvalidOperationException("Connection string 'TestDB' not found.");

            // Configure DbContext options
            _dbOptions = new DbContextOptionsBuilder<BankingDbContext>()
                .UseSqlServer(connectionString).Options;

            // Initialize DataContext and migrate database
            _dbContext = new BankingDbContext(_dbOptions);
            _dbContext.Database.Migrate();

            _accountHelper = new AccountHelperService(_dbContext);
            _accountCreationService = new AccountCreationService(_dbContext, _accountHelper);
            _accountTransactionService = new AccountTransactionService(_dbContext, _accountHelper);
            _transactionsService = new TransactionsService(_dbContext, _accountHelper);
        }

        /// <summary>
        /// Test for creating a saving account.
        /// </summary>
        [Test]
        public async Task CreateSavingAccount_ShouldWork()
        {
            var savingAccountRequest = new CreateSavingAccountRequest
            {
                AccountNumbers = 1234567890,
                Balance = 2000,
                Interest = 5
            };

            var result = await _accountCreationService.CreateSavingAccountAsync(savingAccountRequest);
            var account = await _accountHelper.GetAccountByIdAsync(result.AccountId);

            Assert.That(account.AccountNumbers, Is.EqualTo(savingAccountRequest.AccountNumbers));
            Assert.That(account.Balance, Is.EqualTo(savingAccountRequest.Balance));
            Assert.That(((SavingAccount)account).Interest, Is.EqualTo(savingAccountRequest.Interest));
        }

        /// <summary>
        /// Test for invalid saving account creation.
        /// </summary>
        [Test]
        public async Task InvalidCreateSavingAccount_ShouldThrowException()
        {
            var savingAccountRequest = new CreateSavingAccountRequest
            {
                AccountNumbers = 1234567890,
                Balance = 2000,
                Interest = 5
            };

            await _accountCreationService.CreateSavingAccountAsync(savingAccountRequest);
            Assert.ThrowsAsync<InvalidAccountNumberException>(async () =>
                await _accountCreationService.CreateSavingAccountAsync(savingAccountRequest));
        }

        /// <summary>
        /// Test for withdrawing from a saving account.
        /// </summary>
        [Test]
        public async Task WithdrawFromSavingAccount_ShouldDecreaseBalance()
        {
            var savingAccount = new SavingAccount
            {
                AccountId = Guid.NewGuid(),
                AccountNumbers = 1234567890,
                Balance = 2000,
                Interest = 5
            };

            await _dbContext.Accounts.AddAsync(savingAccount);
            await _dbContext.SaveChangesAsync();

            await _accountTransactionService.WithdrawAsync(savingAccount.AccountId, 500);

            var updatedAccount = await _accountHelper.GetAccountByIdAsync(savingAccount.AccountId);

            Assert.That(updatedAccount.Balance, Is.EqualTo(1500));
        }

        /// <summary>
        /// Test for withdrawing from a saving account exceeding balance.
        /// </summary>
        [Test]
        public void WithdrawFromSavingAccount_ExceedingBalance_ShouldThrow()
        {
            var savingAccount = new SavingAccount
            {
                AccountId = Guid.NewGuid(),
                AccountNumbers = 1234567890,
                Balance = 2000,
                Interest = 5
            };

            _dbContext.Accounts.Add(savingAccount);
            _dbContext.SaveChanges();

            Assert.ThrowsAsync<InvalidAccountOperationException>(async () =>
                await _accountTransactionService.WithdrawAsync(savingAccount.AccountId, 2500));
        }

        /// <summary>
        /// Test for adding interest to a saving account.
        /// </summary>
        [Test]
        public async Task AddInterestToSavingAccount_ShouldIncreaseBalance()
        {
            var savingAccount = new SavingAccount
            {
                AccountId = Guid.NewGuid(),
                AccountNumbers = 1234567890,
                Balance = 2000,
                Interest = 5
            };

            await _dbContext.Accounts.AddAsync(savingAccount);
            await _dbContext.SaveChangesAsync();

            await _accountTransactionService.AddInterestAsync(savingAccount.AccountId, 2);

            var updatedAccount = (SavingAccount)await _accountHelper.GetAccountByIdAsync(savingAccount.AccountId);

            var expectedBalance = 2000 * Math.Pow(1 + (5 / 100.0), 2);
            Assert.That(updatedAccount.Balance, Is.EqualTo((decimal)expectedBalance).Within(0.01m));
        }

        /// <summary>
        /// Test for creating a checking account.
        /// </summary>
        [Test]
        public async Task CreateCheckingAccount_ShouldWork()
        {
            var checkingAccountRequest = new CreateCheckingAccountRequest
            {
                AccountNumbers = 1234567890,
                Balance = 2000,
                Overdrafts = 200 // Optional, will default to 500 if not provided
            };

            var result = await _accountCreationService.CreateCheckingAccountAsync(checkingAccountRequest);
            var account = await _accountHelper.GetAccountByIdAsync(result.AccountId);

            Assert.That(account.AccountNumbers, Is.EqualTo(checkingAccountRequest.AccountNumbers));
            Assert.That(account.Balance, Is.EqualTo(checkingAccountRequest.Balance));
            Assert.That(((CheckingAccount)account).Overdrafts, Is.EqualTo(checkingAccountRequest.Overdrafts));
        }

        /// <summary>
        /// Test for invalid checking account creation.
        /// </summary>
        [Test]
        public async Task InvalidAccountCreation_ShouldThrowException()
        {
            var checkingAccountRequest = new CreateCheckingAccountRequest
            {
                AccountNumbers = 1234567890,
                Balance = 1000,
                Overdrafts = 500
            };

            await _accountCreationService.CreateCheckingAccountAsync(checkingAccountRequest);

            Assert.ThrowsAsync<InvalidAccountNumberException>(async () =>
                await _accountCreationService.CreateCheckingAccountAsync(checkingAccountRequest));
        }

        /// <summary>
        /// Test for withdrawing from a checking account.
        /// </summary>
        [Test]
        public async Task WithdrawFromCheckingAccount_ShouldDecreaseBalance()
        {
            var checkingAccount = new CheckingAccount
            {
                AccountId = Guid.NewGuid(),
                AccountNumbers = 1234567890,
                Balance = 2000,
                Overdrafts = 500
            };

            await _dbContext.Accounts.AddAsync(checkingAccount);
            await _dbContext.SaveChangesAsync();

            await _accountTransactionService.WithdrawAsync(checkingAccount.AccountId, 2300);

            var updatedAccount = await _accountHelper.GetAccountByIdAsync(checkingAccount.AccountId);

            Assert.That(updatedAccount.Balance, Is.EqualTo(-300));
        }

        /// <summary>
        /// Test for withdrawing from a checking account exceeding overdraft limit.
        /// </summary>
        [Test]
        public void WithdrawFromCheckingAccount_ExceedingOverdraft_ShouldThrow()
        {
            var checkingAccount = new CheckingAccount
            {
                AccountId = Guid.NewGuid(),
                AccountNumbers = 1234567890,
                Balance = 2000,
                Overdrafts = 500
            };

            _dbContext.Accounts.Add(checkingAccount);
            _dbContext.SaveChanges();

            Assert.ThrowsAsync<InvalidAccountOperationException>(async () =>
                await _accountTransactionService.WithdrawAsync(checkingAccount.AccountId, 2600));
        }

        /// <summary>
        /// Test for depositing into an account.
        /// </summary>
        [Test]
        public async Task DepositIntoAccount_ShouldIncreaseBalance()
        {
            var checkingAccount = new CheckingAccount
            {
                AccountId = Guid.NewGuid(),
                AccountNumbers = 1234567890,
                Balance = 2000,
                Overdrafts = 500
            };

            await _dbContext.Accounts.AddAsync(checkingAccount);
            await _dbContext.SaveChangesAsync();

            await _accountTransactionService.DepositAsync(checkingAccount.AccountId, 500);

            var updatedAccount = await _accountHelper.GetAccountByIdAsync(checkingAccount.AccountId);

            Assert.That(updatedAccount.Balance, Is.EqualTo(2500));
        }

        /// <summary>
        /// Test for depositing an invalid amount into an account.
        /// </summary>
        [Test]
        public void DepositIntoAccount_InvalidAmount_ShouldThrow()
        {
            var checkingAccount = new CheckingAccount
            {
                AccountId = Guid.NewGuid(),
                AccountNumbers = 1234567890,
                Balance = 2000,
                Overdrafts = 500
            };

            _dbContext.Accounts.Add(checkingAccount);
            _dbContext.SaveChanges();

            Assert.ThrowsAsync<InvalidAccountOperationException>(async () =>
                await _accountTransactionService.DepositAsync(checkingAccount.AccountId, -500));
        }

        /// <summary>
        /// Test for transferring between accounts.
        /// </summary>
        [Test]
        public async Task TransferBetweenAccounts_ShouldWork()
        {
            var sourceAccount = new CheckingAccount
            {
                AccountId = Guid.NewGuid(),
                AccountNumbers = 1234567890,
                Balance = 1000,
                Overdrafts = 500
            };

            var targetAccount = new CheckingAccount
            {
                AccountId = Guid.NewGuid(),
                AccountNumbers = 1234567891,
                Balance = 500,
                Overdrafts = 500
            };

            await _dbContext.Accounts.AddRangeAsync(sourceAccount, targetAccount);
            await _dbContext.SaveChangesAsync();

            await _accountTransactionService.TransferAsync(sourceAccount.AccountId, targetAccount.AccountId, 200);

            var updatedSource = await _accountHelper.GetAccountByIdAsync(sourceAccount.AccountId);
            var updatedTarget = await _accountHelper.GetAccountByIdAsync(targetAccount.AccountId);

            Assert.That(updatedSource.Balance, Is.EqualTo(800));
            Assert.That(updatedTarget.Balance, Is.EqualTo(700));
        }

        /// <summary>
        /// Test for transferring an invalid amount between accounts.
        /// </summary>
        [Test]
        public void TransferBetweenAccounts_InvalidAmount_ShouldThrow()
        {
            var sourceAccount = new CheckingAccount
            {
                AccountId = Guid.NewGuid(),
                AccountNumbers = 1234567890,
                Balance = 1000,
                Overdrafts = 500
            };

            var targetAccount = new CheckingAccount
            {
                AccountId = Guid.NewGuid(),
                AccountNumbers = 1234567891,
                Balance = 500,
                Overdrafts = 500
            };

            _dbContext.Accounts.AddRange(sourceAccount, targetAccount);
            _dbContext.SaveChanges();

            Assert.ThrowsAsync<InvalidAccountOperationException>(async () =>
                await _accountTransactionService.TransferAsync(sourceAccount.AccountId, targetAccount.AccountId, 1200));
        }

        /// <summary>
        /// Test for updating an account.
        /// </summary>
        [Test]
        public async Task UpdateAccount_ShouldWork()
        {
            var checkingAccount = new CheckingAccount
            {
                AccountId = Guid.NewGuid(),
                AccountNumbers = 1234567890,
                Balance = 2000,
                Overdrafts = 500
            };

            await _dbContext.Accounts.AddAsync(checkingAccount);
            await _dbContext.SaveChangesAsync();

            var updateRequest = new UpdateAccountRequest
            {
                Balance = 3000
            };

            var result = await _accountCreationService.UpdateAccountAsync(checkingAccount.AccountId, updateRequest);
            var updatedAccount = await _accountHelper.GetAccountByIdAsync(checkingAccount.AccountId);

            Assert.That(updatedAccount.Balance, Is.EqualTo(updateRequest.Balance));
        }

        /// <summary>
        /// Test for updating an invalid account.
        /// </summary>
        [Test]
        public void UpdateAccount_InvalidAccount_ShouldThrow()
        {
            var updateRequest = new UpdateAccountRequest
            {
                Balance = 0
            };

            Assert.ThrowsAsync<AccountNotFoundException>(async () =>
                await _accountCreationService.UpdateAccountAsync(Guid.NewGuid(), updateRequest));
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}