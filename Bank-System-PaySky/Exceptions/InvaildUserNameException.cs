namespace Bank_System_PaySky.Exceptions
{
    public class InvaildUserNameException : Exception
    {
        public InvaildUserNameException(string message) : base(message)
        {
        }
        public InvaildUserNameException(string message, Exception innerException) : base(message, innerException) { }
    }
}
