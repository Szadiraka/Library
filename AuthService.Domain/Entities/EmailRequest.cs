using AuthService.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AuthService.Domain.Entities
{
    public class EmailRequest
    {
        [Required]
        public EmailMessage Dto { get; set; } = null!;

        [Required]
        public Dictionary<string, string> Data { get; set; } = new();

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EmailTemplate? Template { get; set; }


    }
}
