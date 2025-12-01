using AuthService.Domain.Entities;


namespace AuthService.Application.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(AppUser user, IList<string> roles);

        string GenerateRefreshToken();

        public int TokenExpiration { get; }

        public int RefreshTokenExpiration { get; }

        string GenerateTokenForEmailService();


    }
}
