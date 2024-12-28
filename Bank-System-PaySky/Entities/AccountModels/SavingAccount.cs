using Bank_System_PaySky.Entites.AccountModdels;
using Bank_System_PaySky.Exceptions;

namespace Bank_System_PaySky.Entities.AccountModdels
{
    public class SavingAccount : Account
    {
        public decimal Interest { get; set; }

        public void AddInterest(int years)
        {
            if (years <= 0)
            {
                throw new InvalidAccountOperationException("Years Must be greater than Zero");
            }
            for (int i = 0; i < years; i++)
            {
                Balance += (Interest / 100) * Balance;
            }
        }
        public override void WithDraw(decimal amount)
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
