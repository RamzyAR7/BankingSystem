using Bank_System_PaySky.Models.Accounts;

namespace Bank_System_PaySky.Services
{
    public interface ITransactionsService
    {

        Task<IEnumerable<TransactionResponse>> GetAllTransactionsAsync();
        Task<TransactionResponse> GetTransactionByIdAsync(Guid transactionId);

    }
}
 