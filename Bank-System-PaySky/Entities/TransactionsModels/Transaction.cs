using Bank_System_PaySky.Entities.AccountTransactionsModels;
using System.Data;
namespace Bank_System_PaySky.Entities.TransactionsModels
{
    public enum TransactionType
    {
        Deposit,
        Withdraw,
        Transfer
    }

    public class Transaction
    {
        public Guid TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public virtual ICollection<AccountTransactions> AccountTransactions { get; set; }
    }

}
