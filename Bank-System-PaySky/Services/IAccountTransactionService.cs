using Bank_System_PaySky.Entites.AccountModdels;

namespace Bank_System_PaySky.Services
{
    public interface IAccountTransactionService
    {
        Task<decimal> GetBalanceAsync(Guid accountId);
        Task DepositAsync(Guid accountId, decimal amount);
        Task WithdrawAsync(Guid accountId, decimal amount);
        Task TransferAsync(Guid sourceAccountId, Guid targetAccountId, decimal amount);
        Task AddInterestAsync(Guid accountId, int years);

    }
}
