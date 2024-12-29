using Bank_System_PaySky.Entities.AccountModels;
using Bank_System_PaySky.Entities.AccountTransactionsModels;
using Bank_System_PaySky.Entities.TransactionsModels;

namespace Bank_System_PaySky.Entities.CurrencyModel
{
    public class Currency
    {
        public string CurrencyCode { get; set; }
        // ExchangeRate
        public decimal ExchangeRate { get; set; }
        // isbase
        public bool IsBase { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<AccountTransactions>  AccountTransactions { get; set; }
    }
}
