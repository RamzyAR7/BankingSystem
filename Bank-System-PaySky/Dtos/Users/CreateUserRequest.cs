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
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the email address. Must be a valid email address from a valid domain.
        /// </summary>
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [CustomEmailValidation(ErrorMessage = "Email domain must be from a valid domain.")]
        public string Email { get; set; }

        /// <summary>
        /// 
        /// Gets or sets the password. Must be between 8 and 100 characters, contain at least one uppercase letter, one lowercase letter, one digit, and one special character.
        /// 
        /// </summary>
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,100}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; }
        /// <summary>
        /// 
        /// Gets or sets a value indicating whether the user is an admin. Default is false.
        /// 
        /// </summary>
        [Required(ErrorMessage = "IsAdmin is required.")]
        public bool IsAdmin { get; set; } = false;

    }
}
