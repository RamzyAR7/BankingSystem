using Bank_System_PaySky.Entities.AccountModels;
using Bank_System_PaySky.Models.Accounts;

namespace Bank_System_PaySky.Dtos.Users
{
    public class GetUserResponce
    {
        public Guid UserId { get; set; } // Add this property
        public string UserName { get; set; }
        public string Email { get; set; }

        public bool IsAdmin { get; set; } = false;
        public List<AccountResponse> Accounts { get; set; }
    }
}
