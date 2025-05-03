using Bank_System_PaySky.Entities.AccountModels;

namespace Bank_System_PaySky.Entities.UserModels
{
    public class Users
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}
