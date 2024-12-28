using Bank_System_PaySky.Entites.AccountModdels;
using Bank_System_PaySky.Models.Accounts;

namespace Bank_System_PaySky.Services
{
    public interface IAccountCreationService
    {
        Task<AccountResponse> GetAccountByIdAsync(Guid accountId);
        Task<IEnumerable<AccountResponse>> GetAllAccountsAsync();
        Task<AccountResponse> UpdateAccountAsync(Guid accountId, UpdateAccountRequest account);
        Task DeleteAccountAsync(Guid accountId);
        Task<AccountResponse> CreateCheckingAccountAsync(CreateCheckingAccountRequest account);
        Task<AccountResponse> CreateSavingAccountAsync(CreateSavingAccountRequest account);
    }
}
