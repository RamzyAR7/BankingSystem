using Bank_System_PaySky.Entities.AccountTransactionsModels;
using Bank_System_PaySky.Exceptions;
using Bank_System_PaySky.Exeptions;

namespace Bank_System_PaySky.Entites.AccountModdels
{
    public abstract class Account
    {
        public Guid AccountId { get; set; }
        public double AccountNumbers { get; set; }
        public decimal Balance { get; set; }

        public virtual ICollection<AccountTransactions> AccountTransactions { get; set; }

        public virtual void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new InvalidAccountOperationException("Deposit amount must be greater than zero.");
            }
            Balance += amount;
        }
        public virtual decimal GetBalance()
        {
            return Balance;
        }
        public bool IsTransferValid(decimal amount)
        {
            return Balance >= amount;
        }
        public abstract void WithDraw(decimal amount);

        public void Transfer(Account account, decimal amount)
        {
            if (account == null)
            {
                throw new AccountNotFoundException("Target account is null.");
            }
            if (!IsTransferValid(amount))
            {
                throw new InvalidAccountOperationException("Insufficient funds for the transfer.");
            }
            WithDraw(amount);
            account.Deposit(amount);
        }
    }
}
