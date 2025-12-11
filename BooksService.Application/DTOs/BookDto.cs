using BooksService.Domain.Entities;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace BooksService.Application.DTOs
{
    public class BookDto
    {
        public Guid? Id { get; set; }

        [Required]
        [MinLength(3), MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Range(1900,2100)]
        public int PublishedYear { get; set; }       

        [Required]
        public Guid GenreId { get; set; }
        public Genre? Genre { get; set; }

        public bool IsDeleted { get; set; }

        //[JsonIgnore]
        public List<Guid> AuthorIds { get; set; } = new List<Guid>();


    }
}
