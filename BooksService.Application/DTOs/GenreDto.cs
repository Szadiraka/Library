using BooksService.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;


namespace BooksService.Application.DTOs
{
    public class GenreDto
    {
        public Guid? Id { get; set; }

        [Required]
        [MinLength(3), MaxLength(50)]
        public string Name { get; set; } = string.Empty;

       

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

        public DateTimeOffset? UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset? DeletedAt { get; set; }


        public List<Book> Books { get; set; } = new List<Book>();

    }
}
