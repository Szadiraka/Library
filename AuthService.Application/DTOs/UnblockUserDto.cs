
using System.ComponentModel.DataAnnotations;


namespace AuthService.Application.DTOs
{
    public class UnblockUserDto
    {
        [Required]
        public string UserId { get; set; } = string.Empty;
    }
}
