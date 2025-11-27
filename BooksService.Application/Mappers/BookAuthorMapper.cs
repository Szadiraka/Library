using BooksService.Application.DTOs;
using BooksService.Domain.Entities;


namespace BooksService.Application.Mappers
{
    public static  class BookAuthorMapper
    {

        public static BookAuthor ToBookAuthor(BookAuthorDto dto)
        {
            return new BookAuthor
            {
               AuthorId = dto.AuthorId,
               BookId = dto.BookId

            };
        }

        public static BookAuthorDto ToBookAuthorDto(BookAuthor entity)
        {
            return new BookAuthorDto
            {
                AuthorId =entity.AuthorId,
                BookId = entity.BookId
            };
        }

    }
}
