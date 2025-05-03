using System.ComponentModel.DataAnnotations;

namespace Bank_System_PaySky.Dtos.Users
{
    public class UserLoginDto
    {
        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
        public string Username { get; set; }
        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,100}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; } 
    }
}
