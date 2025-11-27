
using System.ComponentModel.DataAnnotations;


namespace BooksService.Application.DTOs
{
    public class ExistBookDto
    {
        [Required]
        [MinLength(2)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Range(1900, int.MaxValue)]
        public int PublisherYear { get; set; }
    }
}
