using Bank_System_PaySky.Entities.AccountModels;
using Bank_System_PaySky.Exceptions;

namespace Bank_System_PaySky.Entities.AccountModels
{
    public class SavingAccount : Account
    {
        // Interest rate for the savings account
        public decimal Interest { get; set; }

        // Method to add interest to the account balance over a number of years
        public void AddInterest(int years)
        {
            if (years <= 0)
            {
                throw new InvalidAccountOperationException("Years must be greater than zero.");
            }
            
            Balance *= (decimal)Math.Pow((double)(1 + Interest / 100), years);
            
        }

        // Method to withdraw an amount from the account
        public override void Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new InvalidAccountOperationException("Withdraw amount must be greater than zero.");
            }
            if (!IsTransferValid(amount))
            {
                throw new InvalidAccountOperationException("Insufficient funds for the withdrawal.");
            }
            Balance -= amount;
        }
    }
}
