using BooksService.Domain.Entities;
using System;


namespace BooksService.Application.DTOs
{
    public class GenreDto
    {
        public Guid? Id { get; set; }

        public string Name { get; set; } = string.Empty;

        //public List<Book> Books { get; set; } = new List<Book>();

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

    }
}
