using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace AuthService.Application.Services
{
    public class JwtTokenService(IConfiguration configuration) : IJwtTokenService
    {
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var rng = RandomNumberGenerator.Create();
            
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            
        }



        public string GenerateToken(AppUser user, IList<string> roles)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim("username", user.UserName ?? string.Empty),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }



            var keyString = configuration["Jwt:Key"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString!));
           
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

             var token = new JwtSecurityToken(
                 issuer: configuration["Jwt:Issuer"],
                 audience: configuration["Jwt:Audience"],
                 claims: claims,
                 expires: DateTime.UtcNow.AddMinutes(TokenExpirationInMinutes()),
                 signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }


        public int TokenExpirationInMinutes()
        {
            var experies = int.TryParse(configuration["Jwt:TokenExpirationInMinutes"], out var minutes);
            if (!experies)
            {
                minutes = 15;
            }
            return minutes;
        }

        public int RefreshTokenExpirationInDays()
        {
            var experies = int.TryParse(configuration["Jwt:RefreshTokenExpirationInDays"], out var days);
            if (!experies)
            {
                days = 7;
            }
            return days;
        }
    }
}
