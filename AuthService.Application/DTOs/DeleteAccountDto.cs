
using System.ComponentModel.DataAnnotations;


namespace AuthService.Application.DTOs
{
    public class DeleteAccountDto
    {
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
