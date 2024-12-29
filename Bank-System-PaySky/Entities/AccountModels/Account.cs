using Bank_System_PaySky.Entities.AccountTransactionsModels;
using Bank_System_PaySky.Entities.CurrencyModel;
using Bank_System_PaySky.Entities.UserModels;
using Bank_System_PaySky.Exceptions;

namespace Bank_System_PaySky.Entities.AccountModels
{
    public abstract class Account
    {
        // Unique identifier for the account
        public Guid AccountId { get; set; }

        // Account number
        public double AccountNumbers { get; set; }

        // Current balance of the account
        public decimal Balance { get; set; }

        // Collection of account transactions
        public virtual ICollection<AccountTransactions> AccountTransactions { get; set; }

        // Unique identifier for the user foriegn key to the user model
        public Guid UserId { get; set; }
        // Navigation property to the user
        public virtual Users User { get; set; }

        // uniq identifier for the currency foriegn key to the currency model
        public string CurrencyCode { get; set; }
        // Navigation property to the currency
        public virtual Currency Currency { get; set; }


        // Method to deposit an amount into the account
        public virtual void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new InvalidAccountOperationException("Deposit amount must be greater than zero.");
            }
            Balance += amount;
        }

        // Method to get the current balance of the account
        public virtual decimal GetBalance()
        {
            return Balance;
        }

        // Method to check if a transfer is valid based on the amount
        public bool IsTransferValid(decimal amount)
        {
            return Balance >= amount;
        }

        // Abstract method to withdraw an amount from the account
        public abstract void Withdraw(decimal amount);

        // Method to transfer an amount to another account
        public void Transfer(Account targetAccount, decimal amount)
        {
            if (targetAccount == null)
            {
                throw new AccountNotFoundException("Target account is null.");
            }
            if (!IsTransferValid(amount))
            {
                throw new InvalidAccountOperationException("Insufficient funds for the transfer.");
            }
            Withdraw(amount);
            targetAccount.Deposit(amount);
        }
    }
}
