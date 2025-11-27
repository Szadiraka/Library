using BooksService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksService.Application.DTOs
{
    public class BookDto
    {
        public Guid? Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int PublishedYear { get; set; }

        public List<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();

        public Guid GenreId { get; set; }
        public Genre? Genre { get; set; }

        public bool IsDeleted { get; set; }

     
    }
}
