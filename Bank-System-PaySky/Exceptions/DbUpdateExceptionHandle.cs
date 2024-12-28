namespace Bank_System_PaySky.Exceptions
{
    // Custom exception for database update scenarios
    public class DbUpdateExceptionHandle : Exception
    {
        public DbUpdateExceptionHandle(string message) : base(message) { }

        public DbUpdateExceptionHandle(string message, Exception innerException) : base(message, innerException) { }
    }
}
