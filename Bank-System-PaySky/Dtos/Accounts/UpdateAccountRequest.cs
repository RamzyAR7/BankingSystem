using System.ComponentModel.DataAnnotations;

namespace Bank_System_PaySky.Models.Accounts
{
    public class UpdateAccountRequest
    {
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Balance must be a positive value.")]
        public decimal Balance { get; set; }
    }
}
