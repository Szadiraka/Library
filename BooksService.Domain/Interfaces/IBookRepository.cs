using BooksService.Domain.Entities;
using BooksService.Domain.Queries;


namespace BooksService.Domain.Interfaces
{
    public  interface IBookRepository
    {
        Task<Book?> GetBookByIdAsync(Guid id);

        Task<PagedResult<Book>> GetAllBooksAsync(BookQuery query);

        Task<bool> ExistsBookAsync(string title, int PublishedYear);

        Task<Book> AddBookAsync(Book book);

        Task<Book> UpdateBookAsync(Book book);

        Task DeleteBookAsync(Guid id);

        Task<List<Book>> GetAllDeletedBooksAsync();

        Task<bool> RestoreBookAsync(Guid id);

        Task<PagedResult<Book>> GetAllBooksByAuthorIdAsync(BookAuthorQuery query);
    }
}
