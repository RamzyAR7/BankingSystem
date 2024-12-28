using Bank_System_PaySky.Exceptions;

namespace Bank_System_PaySky.Entites.AccountModdels
{
    public class CheckingAccount : Account
    {
        public decimal Overdrafts { get; set; } = 500;

        public override void WithDraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new InvalidAccountOperationException("Withdraw Amount must be greater than zero.");
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
