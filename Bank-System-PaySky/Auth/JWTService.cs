using Bank_System_PaySky.Data;
using Bank_System_PaySky.Entities.UserModels;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace Bank_System_PaySky.Auth
{
    public class JWTService : IJWTService
    {
        private readonly Jwt _option;
        private readonly BankingDbContext _context;
        public JWTService(Jwt option, BankingDbContext context)
        {
            _option = option;
            _context = context;
        }

        public string CreateToken(Users user)
        {
            var TokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _option.Issuer,
                Audience = _option.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_option.SigningKey)), SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.IsAdmin? "admin": "user")
                })
            };
            var securitytoken = TokenHandler.CreateToken(tokenDescriptor);
            var token = TokenHandler.WriteToken(securitytoken);
            return token;
        }
        public string RefreshToken()
        {
            var randomNum = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNum);

            return Convert.ToBase64String(randomNum);
        }
        public string GenrateAndSaveRefreshToken(Users user)
        {
            var refreshToken = RefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_option.Lifetime);
            _context.SaveChanges();
            return refreshToken;
        }
        public Users? ValidateRefreshToken(Guid id, string refreshToken)
        {
            var user = _context.Users.Find(id);

            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }
            return user;
        }
    }
}
