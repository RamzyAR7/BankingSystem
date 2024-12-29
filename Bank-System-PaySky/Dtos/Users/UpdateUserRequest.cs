using System.ComponentModel.DataAnnotations;

namespace Bank_System_PaySky.Dtos.Users
{
    public class UpdateUserRequest
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [CustomEmailValidation(ErrorMessage = "Email domain must be from a valid domain.")]
        public string? Email { get; set; }
    }
}
