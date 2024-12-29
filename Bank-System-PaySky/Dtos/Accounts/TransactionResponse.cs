namespace Bank_System_PaySky.Models.Accounts
{
    public class TransactionResponse
    {
        public Guid TransactionId { get; set; }
        public Guid? SourceAccountId { get; set; }
        public Guid? TargetAccountId { get; set; }
        public string? SourceCurrancyType { get; set; }
        public string? TargetCurrancyType { get; set; }
        public string TypeOfOperation { get; set; }
        public decimal AmountToTarget { get; set; }
        public DateTime Timestamp { get; set; }
    }

}
