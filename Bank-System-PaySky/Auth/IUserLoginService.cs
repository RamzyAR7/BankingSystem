using Bank_System_PaySky.Dtos.Users;

namespace Bank_System_PaySky.Auth
{
    public interface IUserLoginService
    {
        Task<TokenResDto?> LoginAsync(UserLoginDto request);
        TokenResDto RefreshToken(RefreshTokenReqDto refreshTokenReqDto);

    }
}
