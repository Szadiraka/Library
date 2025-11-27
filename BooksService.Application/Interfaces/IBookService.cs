

using BooksService.Application.DTOs;
using BooksService.Domain.Interfaces;
using BooksService.Domain.Queries;

namespace BooksService.Application.Interfaces
{
    public  interface IBookService
    {
        Task<BookDto> GetBookByIdAsync(Guid id);

        Task<PagedResult<BookDto>> GetAllBooksAsync(BookQuery query);

        Task<bool> ExistsBookAsync(string title, int PublishedYear);

        Task<BookDto> AddBookAsync(BookDto book);

        Task<BookDto> UpdateBookAsync(Guid id, BookDto book);

        Task DeleteBookAsync(Guid id);

        Task<List<BookDto>> GetAllDeletedBooksAsync();

        Task<bool> RestoreBookAsync(Guid id);

        Task<PagedResult<BookDto>> GetAllBooksByAuthorIdAsync(BookAuthorQuery query);
    }
}
