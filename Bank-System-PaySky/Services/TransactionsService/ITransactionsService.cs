using Bank_System_PaySky.Models.Accounts;

namespace Bank_System_PaySky.Services.Transactions
{
    public interface ITransactionsService
    {
        // Method to get all transactions
        Task<IEnumerable<TransactionResponse>> GetAllTransactionsAsync();

        // Method to get transaction details by transaction ID
        Task<TransactionResponse> GetTransactionByIdAsync(Guid transactionId);
    }
}
