
using EmailService.Domain.Domains;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace EmailService.Application.DTOs
{
    public class EmailRequest
    {
        [Required]
        public EmailMessageDto Dto { get; set; } = null!;

        [Required]
        public Dictionary<string, string> Data { get; set; } = new ();

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EmailTemplate? Template { get; set; }
    }
}
