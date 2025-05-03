
using Bank_System_PaySky.Entities.UserModels;

namespace Bank_System_PaySky.Auth
{
    public interface IJWTService
    {
        string CreateToken(Users user);
        string GenrateAndSaveRefreshToken(Users user);
        Users? ValidateRefreshToken(Guid id, string refreshToken);
    }
}
