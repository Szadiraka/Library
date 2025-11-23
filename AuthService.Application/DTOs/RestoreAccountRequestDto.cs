
using System.ComponentModel.DataAnnotations;

namespace AuthService.Application.DTOs
{
    public class RestoreAccountRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
