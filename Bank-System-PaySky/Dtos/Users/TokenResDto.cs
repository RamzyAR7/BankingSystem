namespace Bank_System_PaySky.Dtos.Users
{
    public class TokenResDto
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
