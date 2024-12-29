using Bank_System_PaySky.Dtos.Users;
using Bank_System_PaySky.Entities.UserModels;

namespace Bank_System_PaySky.Services.UserCreationService
{
    public interface IUserCreationService
    {
        // Method to create a new saving User
        public Task<GetUserResponce> CreateUserAsync(CreateUserRequest user);

        // Method to get account details by User ID
        public Task<GetUserResponce> GetUserAsync(Guid userId);

        // Method to get all Users
        public Task<IEnumerable<GetUserResponce>> GetUsersAsync();

        // Method to update an existing User
        public Task<GetUserResponce> UpdateUserAsync(Guid userId, UpdateUserRequest user);

        // Method to delete an User
        public Task DeleteUserAsync(Guid userId);
    }
}
