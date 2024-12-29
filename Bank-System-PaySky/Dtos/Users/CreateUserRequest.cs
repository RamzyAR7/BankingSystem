using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Bank_System_PaySky.Dtos.Users
{
    /// <summary>
    /// Request model for creating a user.
    /// </summary>
    public class CreateUserRequest
    {
        /// <summary>
        /// Gets or sets the username. Must be between 3 and 50 characters.
        /// </summary>
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the email address. Must be a valid email address from a valid domain.
        /// </summary>
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [CustomEmailValidation(ErrorMessage = "Email domain must be from a valid domain.")]
        public string Email { get; set; }
    }
}
