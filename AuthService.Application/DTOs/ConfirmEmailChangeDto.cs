
using System.ComponentModel.DataAnnotations;


namespace AuthService.Application.DTOs
{
    public class ConfirmEmailChangeDto
    {
        [Required]
        [EmailAddress]
        public string NewEmail { get; set; }= string.Empty;

        [Required]
        public string Token { get; set; } = string.Empty;
    }
}
