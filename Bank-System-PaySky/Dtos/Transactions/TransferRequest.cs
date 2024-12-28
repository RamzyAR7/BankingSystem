using System.ComponentModel.DataAnnotations;

namespace Bank_System_PaySky.Models.Transactions
{
    public class TransferRequest
    {
        [Required]
        public Guid SourceAccountId { get; set; }

        [Required]
        public Guid TargetAccountId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be a positive value.")]
        public decimal Amount { get; set; }
    }
}
