using Bank_System_PaySky.Entities.AccountModels;
using Bank_System_PaySky.Models.Accounts;

namespace Bank_System_PaySky.Services.AccountCreation
{
    public interface IAccountCreationService
    {
        // Method to get account details by account ID
        Task<AccountResponse> GetAccountByIdAsync(Guid accountId);

        // Method to get all accounts
        Task<IEnumerable<AccountResponse>> GetAllAccountsAsync();

        // Method to update an existing account
        Task<AccountResponse> UpdateAccountAsync(Guid accountId, UpdateAccountRequest account);

        // Method to delete an account
        Task DeleteAccountAsync(Guid accountId);

        // Method to create a new checking account
        Task<AccountResponse> CreateCheckingAccountAsync(CreateCheckingAccountRequest account);

        // Method to create a new saving account
        Task<AccountResponse> CreateSavingAccountAsync(CreateSavingAccountRequest account);
    }
}
