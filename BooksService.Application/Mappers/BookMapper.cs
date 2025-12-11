
using BooksService.Application.DTOs;
using BooksService.Domain.Entities;
using BooksService.Domain.Interfaces;

namespace BooksService.Application.Mappers
{
    public static class BookMapper
    {
        public static Book ToBook(BookDto dto)
        {
            return new Book
            {             
                Title = dto.Title,
                Description = dto.Description,
                PublishedYear = dto.PublishedYear,            
                GenreId = dto.GenreId, 
                IsDeleted = dto.IsDeleted,
                CreatedAt = DateTimeOffset.UtcNow,
                
            };
        }

        public static BookDto ToBookDto(Book book)
        {
            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                PublishedYear = book.PublishedYear,              
                GenreId = book.GenreId, 
                Genre = book.Genre,
                IsDeleted = book.IsDeleted,
                AuthorIds = book.BookAuthors.Select(x=> x.AuthorId).ToList(),
                
            };
        }

        public static PagedResult<BookDto> MapToDtoList(PagedResult<Book> entities)
        {
            return new PagedResult<BookDto>
            {
                Page = entities.Page,
                PageSize = entities.PageSize,
                Items = entities.Items.Select(x => ToBookDto(x)).ToList(),
                TotalCount = entities.TotalCount
            };

        }

        public static void UpdateData(Book entity, BookDto dto)
        {
            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.PublishedYear = dto.PublishedYear;         
            entity.GenreId = dto.GenreId;           
            entity.UpdatedAt = DateTimeOffset.Now;

        }

        public static List<BookDto> MatToSimpleListDto(List<Book> entities)
        {
            return entities.Select(x => ToBookDto(x)).ToList();
        }
    }
}
