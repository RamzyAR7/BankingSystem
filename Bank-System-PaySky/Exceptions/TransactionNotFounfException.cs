namespace Bank_System_PaySky.Exceptions
{
    public class TransactionNotFounfException:Exception
    {
        public TransactionNotFounfException()
        {
            
        }
        public TransactionNotFounfException(string message) : base(message) { }

        public TransactionNotFounfException(string message, Exception innerException) : base(message, innerException) { }
    }
}
