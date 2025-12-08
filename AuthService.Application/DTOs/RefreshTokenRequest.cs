
using System.ComponentModel.DataAnnotations;


namespace AuthService.Application.DTOs
{
    public  class RefreshTokenRequest
    {

        [Required]
       public  string Token { get; set; } = string.Empty;

        [Required]
       public  string RefreshToken { get; set; } = string.Empty;
    }
}
