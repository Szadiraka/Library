
using System.ComponentModel.DataAnnotations;


namespace AuthService.Application.DTOs
{
    public  class ChangePasswordDto
    {
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        public string NewPassword { get; set; } = string.Empty;
    }
}
