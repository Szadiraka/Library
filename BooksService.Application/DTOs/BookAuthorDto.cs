
using System.ComponentModel.DataAnnotations;


namespace BooksService.Application.DTOs
{
    public class BookAuthorDto
    {
        [Required]
        public Guid BookId { get; set; }

        [Required]
        public Guid AuthorId { get; set; }
    }
}
