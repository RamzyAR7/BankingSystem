using Bank_System_PaySky.Entities.AccountModels;
using Bank_System_PaySky.Exceptions;

namespace Bank_System_PaySky.Entities.AccountModels
{
    public class CheckingAccount : Account
    {
        // Overdraft limit for the checking account
        public decimal Overdrafts { get; set; } = 500;

        // Method to withdraw an amount from the account
        public override void Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new InvalidAccountOperationException("Withdraw amount must be greater than zero.");
            }
            else if (Balance + Overdrafts >= amount)
            {
                Balance -= amount;
            }
            else
            {
                throw new InvalidAccountOperationException("Withdrawal exceeds account balance and overdraft limit.");
            }
        }
    }
}
