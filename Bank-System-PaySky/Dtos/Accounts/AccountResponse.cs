namespace Bank_System_PaySky.Models.Accounts
{
    public class AccountResponse
    {
        public Guid AccountId { get; set; }
        public double AccountNumbers { get; set; }
        public string AccountType { get; set; }
        public decimal Balance { get; set; }
        public decimal? Interest { get; set; }
        public decimal? Overdrafts { get; set; }
    }
}
