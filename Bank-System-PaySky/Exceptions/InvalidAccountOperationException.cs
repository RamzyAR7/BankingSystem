namespace Bank_System_PaySky.Exceptions
{
    // Custom exception for invalid account operation scenarios
    public class InvalidAccountOperationException : Exception
    {
        public InvalidAccountOperationException(string message) : base(message) { }

        public InvalidAccountOperationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
