

namespace EmailService.Domain.Entities
{
    public  class EmailMessage
    {
        public Guid Id { get; set; }

        public Guid? UserId { get; set; }

        public string To { get; set; } =  string.Empty;

        public string Subject { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset? SentAt { get; set; }

        public bool IsSent { get; set; } = false;

        public string? ErrorMessage { get; set; }

       
    }
}
