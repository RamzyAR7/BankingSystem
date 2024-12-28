using Bank_System_PaySky.Entites.AccountModdels;
using Bank_System_PaySky.Entities.TransactionsModels;

namespace Bank_System_PaySky.Services
{
    public interface IAccountHelperService
    {
        Task<Account> GetAccountByIdAsync(Guid accountId);
        Task<Transaction> GetTransactionByIdAsync(Guid transactionId);
        Task<bool> IsSavingAccountAsync(Guid accountId);
    }

}
