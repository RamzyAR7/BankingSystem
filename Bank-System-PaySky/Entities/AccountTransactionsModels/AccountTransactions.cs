using Bank_System_PaySky.Entites.AccountModdels;
using Bank_System_PaySky.Entities.TransactionsModels;

namespace Bank_System_PaySky.Entities.AccountTransactionsModels
{
    public class AccountTransactions
    {
        public Guid AccountId { get; set; }
        public virtual Account Account { get; set; } // navigation prop
        public Guid TransactionId { get; set; }
        public virtual Transaction Transaction { get; set; }// navigation prop
        public string AccountStatus { get; set; }
    }
}
