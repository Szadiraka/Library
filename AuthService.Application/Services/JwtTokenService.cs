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
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;         

        public JwtTokenService(IConfiguration configuration)
        {
                _configuration = configuration;          

        }

        public int TokenExpiration {
            get
            {
                var result = int.TryParse(_configuration["Jwt:TokenExpirationInMinutes"], out var minutes);
                if (!result)
                {
                    minutes = 15;
                }
                return minutes;
            }
        }

        public int RefreshTokenExpiration
        {
            get
            {
                var result = int.TryParse(_configuration["Jwt:RefreshTokenExpirationInDays"], out var days);
                if (!result)
                {
                    days = 7;
                }
                return days;
            }
        }



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

            var keyString = _configuration["Jwt:Key"];         


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
           
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

             var token = new JwtSecurityToken(
                 issuer: _configuration["Jwt:Issuer"],
                 audience: _configuration["Jwt:Audience"],
                 claims: claims,
                 expires: DateTime.UtcNow.AddMinutes(TokenExpiration),
                 signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public string GenerateTokenForEmailService()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "AuthService"),
                new Claim("iss", "AuthService"),                
            };

          

            var keyString = _configuration["Jwt:Key"];


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: credentials
           );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


      
    }
}
