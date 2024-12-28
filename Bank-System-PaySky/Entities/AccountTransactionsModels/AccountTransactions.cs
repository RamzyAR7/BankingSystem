using Bank_System_PaySky.Entities.AccountModels;
using Bank_System_PaySky.Entities.TransactionsModels;

namespace Bank_System_PaySky.Entities.AccountTransactionsModels
{
    public class AccountTransactions
    {
        // Unique identifier for the account
        public Guid AccountId { get; set; }

        // Navigation property to the account
        public virtual Account Account { get; set; }

        // Unique identifier for the transaction
        public Guid TransactionId { get; set; }

        // Navigation property to the transaction
        public virtual Transaction Transaction { get; set; }

        // Status of the account
        public string AccountStatus { get; set; }
    }
}
