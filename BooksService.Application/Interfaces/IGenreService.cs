

using BooksService.Application.DTOs;
using BooksService.Domain.Interfaces;
using BooksService.Domain.Queries;

namespace BooksService.Application.Interfaces
{
    public interface IGenreService
    {
        Task<GenreDto> GetGenreByIdAsync(Guid id);

        Task<PagedResult<GenreDto>> GetAllGenresAsync(GenreQuery query);

        Task<bool> ExistsGenreAsync(string name);

        Task<GenreDto> AddGenreAsync(GenreDto dto);

        Task<GenreDto> UpdateGenreAsync(Guid id, GenreDto dto);

        Task DeleteGenreAsync(Guid id);

        Task<List<GenreDto>> GetAllDeletedGenresAsync();

        Task<bool> RestoreGenreAsync(Guid id);
    }
}
