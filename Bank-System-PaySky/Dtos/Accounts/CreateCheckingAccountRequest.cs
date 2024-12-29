using Bank_System_PaySky.Dtos.Accounts;
using Bank_System_PaySky.Entities.CurrencyModel;
using System.ComponentModel.DataAnnotations;

namespace Bank_System_PaySky.Models.Accounts
{
    /// <summary>
    /// Request model for creating a checking account.
    /// </summary>
    public class CreateCheckingAccountRequest
    {
        /// <summary>
        /// Gets or sets the account number. Must be a 10-digit number.
        /// </summary>
        [Required]
        [Range(1000000000, 9999999999, ErrorMessage = "Account number must be a 10-digit number.")]
        public double AccountNumbers { get; set; }

        /// <summary>
        /// Gets or sets the balance. Must be a positive value.
        /// </summary>
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Balance must be a positive value.")]
        public decimal Balance { get; set; }

        /// <summary>
        /// Gets or sets the overdrafts. Must be a positive value.
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Overdrafts must be a positive value.")]
        public decimal? Overdrafts { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCheckingAccountRequest"/> class.
        /// Sets the default value for overdrafts to 500.
        /// </summary>
        public CreateCheckingAccountRequest()
        {
            Overdrafts = 500; // Set default value for Overdrafts
        }

        /// <summary>
        /// Gets or sets the currency code. Must be one of the following: USD, EGP, SAR, EUR, GBP.
        /// </summary>
        [Required(ErrorMessage = "Currency is required.")]
        [CurrencyCodeValidation]
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        [Required(ErrorMessage = "User ID is required.")]
        public Guid UserId { get; set; }
    }
}
