namespace Bank_System_PaySky.Services.AccountTransactionService
{
    public interface IAccountTransactionService
    {
        // Method to get the balance of an account by account ID
        Task<decimal> GetBalanceAsync(Guid accountId);

        // Method to deposit an amount into an account
        Task DepositAsync(Guid accountId, decimal amount);

        // Method to withdraw an amount from an account
        Task WithdrawAsync(Guid accountId, decimal amount);

        // Method to transfer an amount from one account to another
        Task TransferAsync(Guid sourceAccountId, Guid targetAccountId, decimal amount);

        // Method to add interest to a saving account
        Task AddInterestAsync(Guid accountId, int years);
    }
}
