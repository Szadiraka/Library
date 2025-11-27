using BooksService.Domain.Entities;
using BooksService.Domain.Queries;


namespace BooksService.Domain.Interfaces
{
    public interface IGenreRepository
    {
        Task<Genre?> GetGenreByIdAsync(Guid id);

        Task<PagedResult<Genre>> GetAllGenresAsync(GenreQuery query);

        Task<bool> ExistsGenreAsync(string name);

        Task<Genre> AddGenreAsync(Genre genre);

        Task<Genre> UpdateGenreAsync(Genre genre);

        Task DeleteGenreAsync(Guid id);

        Task<List<Genre>> GetAllDeletedGenresAsync();

        Task<bool> RestoreGenreAsync(Guid id);

    }
}
