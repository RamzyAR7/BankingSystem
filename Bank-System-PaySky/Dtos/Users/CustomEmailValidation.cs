using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Bank_System_PaySky.Dtos.Users
{
    /// <summary>
    /// Custom validation attribute for email domain.
    /// </summary>
    public class CustomEmailValidation : ValidationAttribute
    {
        /// <summary>
        /// Validates the specified email address to ensure it has a valid domain.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>true if the email domain is valid; otherwise, false.</returns>
        public override bool IsValid(object value)
        {
            var email = value as string;
            if (string.IsNullOrEmpty(email))
                return false;

            // Basic regex to match valid email domain, e.g., only allows example.com or .org
            var domainRegex = new Regex(@"@([a-zA-Z0-9-]+)\.(com|org)$");
            return domainRegex.IsMatch(email);
        }
    }
}
