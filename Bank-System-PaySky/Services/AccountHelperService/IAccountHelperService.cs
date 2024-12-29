using Bank_System_PaySky.Entities.AccountModels;
using Bank_System_PaySky.Entities.TransactionsModels;

namespace Bank_System_PaySky.Services.AccountHelper
{
    public interface IAccountHelperService
    {
        Task<Account> GetAccountByIdAsync(Guid accountId);
        Task<Transaction> GetTransactionByIdAsync(Guid transactionId);
        Task<bool> IsSavingAccountAsync(Guid accountId);
        Task<decimal> ConvertAsync(string fromCurrencyId, string toCurrencyId, decimal amount);

    }

}
