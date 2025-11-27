using BooksService.Application.DTOs;


namespace BooksService.Application.Interfaces
{
    public interface IBookAuthorService
    {

        Task<bool> AddBookAuthorAsync(BookAuthorDto dto);

        Task DeleteBookAuthorAsync(BookAuthorDto dto);

        Task<List<Guid>> GetAuthorsIdsByBookIdAsync(Guid bookId);

       
    }
}
