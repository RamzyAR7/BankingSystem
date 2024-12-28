namespace Bank_System_PaySky.Exceptions
{
    public class InvalidAccountOperationException:Exception
    {
        public InvalidAccountOperationException()
        {
            
        }
        public InvalidAccountOperationException(string message) : base(message) { }
        public InvalidAccountOperationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
