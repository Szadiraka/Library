

using System.ComponentModel.DataAnnotations;

namespace EmailService.Application.DTOs
{
    public class EmailMessageDto
    {
        public Guid? Id { get; set; }

        public Guid? UserId { get; set; }

        [Required]
        [EmailAddress]
        public string To { get; set; } = string.Empty;

        [Required]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string Body { get; set; } = string.Empty;

        public DateTimeOffset? CreatedAt { get; set; }

        public DateTimeOffset? SentAt { get; set; }
        
        public bool IsSent { get; set; } = false;

        public string? ErrorMessage { get; set; }
    }
}
