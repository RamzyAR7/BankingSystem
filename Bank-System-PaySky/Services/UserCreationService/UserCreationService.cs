using Bank_System_PaySky.Data;
using Bank_System_PaySky.Dtos.Users;
using Bank_System_PaySky.Entities.AccountModels;
using Bank_System_PaySky.Entities.UserModels;
using Bank_System_PaySky.Exceptions;
using Bank_System_PaySky.Models.Accounts;
using Bank_System_PaySky.Services.AccountCreation;
using Microsoft.EntityFrameworkCore;

namespace Bank_System_PaySky.Services.UserCreationService
{
    public class UserCreationService : IUserCreationService
    {

        private readonly BankingDbContext _dbContext;

        public UserCreationService(BankingDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<GetUserResponce> GetUserAsync(Guid userId)
        {
            var user = await _dbContext.Users
                .Include(u => u.Accounts)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                throw new UserNotFoundException("Users not found"); // or throw an exception if user not found
            }
            var response = new GetUserResponce
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                // Password should not be exposed in the response
                Accounts = user.Accounts.Select(a => new AccountResponse
                {
                    AccountId = a.AccountId,
                    AccountNumbers = a.AccountNumbers,
                    Balance = a.Balance,
                    CurrencyCode = a.CurrencyCode,
                    Interest = a is SavingAccount savingAccount ? savingAccount.Interest : null,
                    Overdrafts = a is CheckingAccount checkingAccount ? checkingAccount.Overdrafts : null,
                    UserId = a.UserId
                }).ToList()
            };

            return response;
        }

        public async Task<IEnumerable<GetUserResponce>> GetUsersAsync()
        {
            var users = await _dbContext.Users
               .Include(u => u.Accounts)
               .ToListAsync();

            var response = users.Select(user => new GetUserResponce
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                // Password should not be exposed in the response
                Accounts = user.Accounts.Select(a => new AccountResponse
                {
                    AccountId = a.AccountId,
                    AccountNumbers = a.AccountNumbers,
                    Balance = a.Balance,
                    CurrencyCode = a.CurrencyCode,
                    Interest = a is SavingAccount savingAccount ? savingAccount.Interest : null,
                    Overdrafts = a is CheckingAccount checkingAccount ? checkingAccount.Overdrafts : null,
                    UserId = a.UserId

                }).ToList()
            });
            return response;
        }
        public async Task<GetUserResponce> CreateUserAsync(CreateUserRequest user)
        {
            if (user == null)
            {
                throw new InvalidAccountNumberException("User details are required.");
            }

            var existingUser = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Username == user.Username);

            if (existingUser != null)
            {
                throw new InvalidAccountNumberException("User with the same username already exists.");
            }

            var newUser = new Users
            {
                UserId = Guid.NewGuid(), // Auto-generate UserId
                Username = user.Username,
                Email = user.Email
            };
            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();

            var response = new GetUserResponce
            {
                UserId = newUser.UserId,
                Username = newUser.Username,
                Email = newUser.Email
            };
            return response;
        }

        public async Task<GetUserResponce> UpdateUserAsync(Guid userId, UpdateUserRequest user)
        {
            // Check if all fields are null or empty
            if (string.IsNullOrEmpty(user.Username) && string.IsNullOrEmpty(user.Email))
            {
                throw new InvaildUserNameException("User details are required.");
            }

            var existingUser = await _dbContext.Users
                    .Include(u => u.Accounts)
                    .FirstOrDefaultAsync(u => u.UserId == userId);

            if (existingUser == null)
            {
                throw new UserNotFoundException("User not found");
            }

            // Update the user details only if they are provided
            if (!string.IsNullOrEmpty(user.Username))
            {
                existingUser.Username = user.Username;
            }
            if (!string.IsNullOrEmpty(user.Email))
            {
                existingUser.Email = user.Email;
            }

            // Save the changes to the database
            await _dbContext.SaveChangesAsync();

            // Return the updated user details
            var response = new GetUserResponce
            {
                UserId = existingUser.UserId,
                Username = existingUser.Username,
                Email = existingUser.Email,
                // Password should not be exposed in the response
                Accounts = existingUser.Accounts.Select(a => new AccountResponse
                {
                    AccountId = a.AccountId,
                    AccountNumbers = a.AccountNumbers,
                    Balance = a.Balance,
                    CurrencyCode = a.CurrencyCode,
                    Interest = a is SavingAccount savingAccount ? savingAccount.Interest : null,
                    Overdrafts = a is CheckingAccount checkingAccount ? checkingAccount.Overdrafts : null,
                    UserId = a.UserId
                }).ToList()
            };
            return response;
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            // Check if the user exists
            var existingUser = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (existingUser == null)
            {
                throw new UserNotFoundException("User not found");
            }

            // Delete the user from the database
            _dbContext.Users.Remove(existingUser);
            await _dbContext.SaveChangesAsync();
        }
    }
}
