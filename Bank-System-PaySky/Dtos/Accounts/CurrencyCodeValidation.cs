using System.ComponentModel.DataAnnotations;

namespace Bank_System_PaySky.Dtos.Accounts
{
    /// <summary>
    /// Validates that the currency code is one of the allowed values: USD, EGP, SAR, EUR, GBP.
    /// </summary>
    public class CurrencyCodeValidation : ValidationAttribute
    {
        private readonly string[] _allowedCurrencies = { "USD", "EGP", "SAR", "EUR", "GBP" };

        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>An instance of the <see cref="ValidationResult"/> class.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string currencyCode && _allowedCurrencies.Contains(currencyCode))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult($"Currency code must be one of the following: {string.Join(", ", _allowedCurrencies)}.");
        }
    }
}
