namespace Bank_System_PaySky.Exceptions
{
    public class UserNotFoundException:Exception
    {
        public UserNotFoundException(string message) : base(message) { }
        public UserNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
