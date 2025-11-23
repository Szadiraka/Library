
using System.ComponentModel.DataAnnotations;


namespace AuthService.Application.DTOs
{
    public  class ChangeEmailRequestDto
    {
        [Required]
        [EmailAddress]
        public string NewEmail { get; set; } = string.Empty;
    }
}
