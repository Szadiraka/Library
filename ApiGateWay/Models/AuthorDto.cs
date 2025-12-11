using System.ComponentModel.DataAnnotations;

namespace ApiGateWay.Models
{
    public class AuthorDto
    {
        public Guid? Id { get; set; }

        [Required]
        [MinLength(5)]
        public string FullName { get; set; } = string.Empty;

        public string? Bio { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly BirthDate { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? DeathDate { get; set; }

        public string? Nationality { get; set; }
    }
}
