using System.ComponentModel.DataAnnotations;

namespace Bank_System_PaySky.Models.Accounts
{
    public class CreateCheckingAccountRequest
    {
        [Required]
        [Range(1000000000, 9999999999, ErrorMessage = "Account number must be a 10-digit number.")]
        public double AccountNumber { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Balance must be a positive value.")]
        public decimal Balance { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Overdrafts must be a positive value.")]
        public decimal? Overdrafts { get; set; }

        public CreateCheckingAccountRequest()
        {
            Overdrafts = 500; // Set default value for Overdrafts
        }

    }
}
