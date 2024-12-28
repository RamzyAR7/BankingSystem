namespace Bank_System_PaySky.Exceptions
{
    // Custom exception for transaction not found scenarios
    public class TransactionNotFoundException : Exception
    {
        public TransactionNotFoundException(string message) : base(message) { }

        public TransactionNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
