namespace Bank_System_PaySky.Exceptions
{
    // Custom exception for invalid account number scenarios
    public class InvalidAccountNumberException : Exception
    {
        public InvalidAccountNumberException(string message) : base(message) { }

        public InvalidAccountNumberException(string message, Exception innerException) : base(message, innerException) { }
    }
}
