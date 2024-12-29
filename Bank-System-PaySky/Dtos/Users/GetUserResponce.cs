using Bank_System_PaySky.Entities.AccountModels;
using Bank_System_PaySky.Models.Accounts;

namespace Bank_System_PaySky.Dtos.Users
{
    public class GetUserResponce
    {
        public Guid UserId { get; set; } // Add this property
        public string Username { get; set; }
        public string Email { get; set; }
        public List<AccountResponse> Accounts { get; set; }
    }
}
