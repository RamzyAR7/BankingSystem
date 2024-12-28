using System.ComponentModel.DataAnnotations;

namespace Bank_System_PaySky.Models.Transactions
{
    public class WithdrawRequest
    {
        [Required]
        public Guid AccountId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be a positive value.")]
        public decimal Amount { get; set; }
    }

}
