namespace Bank_System_PaySky.Exceptions
{
    // Custom exception for account not found scenarios
    public class AccountNotFoundException : Exception
    {
        public AccountNotFoundException(string message) : base(message) { }

        public AccountNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
