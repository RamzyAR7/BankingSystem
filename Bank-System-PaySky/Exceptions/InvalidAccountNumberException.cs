namespace Bank_System_PaySky.Exceptions
{
    public class InvalidAccountNumberException: Exception
    {
        public InvalidAccountNumberException()
        {
            
        }
        public InvalidAccountNumberException(string message):base(message) { }

        public InvalidAccountNumberException(string message, Exception innerException) : base(message, innerException) { }
    }
}
