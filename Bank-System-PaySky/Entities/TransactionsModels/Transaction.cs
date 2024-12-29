using Bank_System_PaySky.Entities.AccountTransactionsModels;
using Bank_System_PaySky.Entities.CurrencyModel;
using System.Data;

namespace Bank_System_PaySky.Entities.TransactionsModels
{
    // Enum to represent the type of transaction
    public enum TransactionType
    {
        Deposit,
        Withdraw,
        Transfer
    }

    public class Transaction
    {
        // Unique identifier for the transaction
        public Guid TransactionId { get; set; }
        // Amount involved in the transaction
        public decimal Amount { get; set; }
        // Type of the transaction (Deposit, Withdraw, Transfer)
        public string TransactionType { get; set; }
        // Timestamp of when the transaction occurred
        public DateTime Timestamp { get; set; }
        // Collection of account transactions associated with this transaction
        public virtual ICollection<AccountTransactions> AccountTransactions { get; set; }
    }

}
