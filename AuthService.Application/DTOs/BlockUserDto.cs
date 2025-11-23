

using System.ComponentModel.DataAnnotations;

namespace AuthService.Application.DTOs
{
    public class BlockUserDto
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string Reason { get; set; } = string.Empty;

        public DateTime? ExpiresAt { get; set; }
    }
}
