namespace Bank_System_PaySky.Exceptions
{
    public class InvaildUserOperationException : Exception
    {
        public InvaildUserOperationException(string message) : base(message)
        {
        }
        public InvaildUserOperationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
