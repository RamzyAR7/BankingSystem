using Bank_System_PaySky.Data;
using Bank_System_PaySky.Dtos.Users;
using Bank_System_PaySky.Entities.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace Bank_System_PaySky.Auth
{
    public class UserLoginService : IUserLoginService
    {
        private readonly BankingDbContext _context;
        private readonly IJWTService _jwtService;


        public UserLoginService(BankingDbContext context, IJWTService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<TokenResDto?> LoginAsync(UserLoginDto request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == request.Username);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            if (BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return CreateTokenRes(user);
            }
            else
            {
                return null;
            }
        }

        public TokenResDto RefreshToken(RefreshTokenReqDto refreshTokenReqDto)
        {
            var user = _jwtService.ValidateRefreshToken(refreshTokenReqDto.Id, refreshTokenReqDto.RefreshToken);
            if (user is null)
            {
                return null;
            }
            return CreateTokenRes(user);
        }
        private TokenResDto CreateTokenRes(Users user)
        {
            var fullToken = new TokenResDto
            {
                AccessToken = _jwtService.CreateToken(user),
                RefreshToken = _jwtService.GenrateAndSaveRefreshToken(user)
            };
            return fullToken;
        }
        //public IList<Users> GetUsers()
        //{
        //    var users = _context.Users.ToList();
        //    return users;
        //}
    }
}
