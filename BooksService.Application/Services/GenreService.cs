using BooksService.Application.DTOs;
using BooksService.Application.Exceptions;
using BooksService.Application.Interfaces;
using BooksService.Application.Mappers;
using BooksService.Domain.Entities;
using BooksService.Domain.Interfaces;
using BooksService.Domain.Queries;


namespace BooksService.Application.Services
{
    public class GenreService : IGenreService
    {

        private readonly IGenreRepository _repo;

        public GenreService(IGenreRepository repo)
        {
            _repo = repo;
        }



        public async Task<GenreDto> AddGenreAsync(GenreDto dto)
        {
            var result = await _repo.AddGenreAsync(GenreMapper.ToGenre(dto));

            return GenreMapper.ToGenreDto(result);
        }


        public async Task DeleteGenreAsync(Guid id)
        {
            await _repo.DeleteGenreAsync(id);
        }


        public async Task<bool> ExistsGenreAsync(string name)
        {
            return await _repo.ExistsGenreAsync(name);
        }


        public async Task<List<GenreDto>> GetAllDeletedGenresAsync()
        {
            var result = await _repo.GetAllDeletedGenresAsync();

            return GenreMapper.MatToSimpleListDto(result);
        }


        public async Task<PagedResult<GenreDto>> GetAllGenresAsync(GenreQuery query)
        {
            PagedResult<Genre> result = await _repo.GetAllGenresAsync(query);

            var list = GenreMapper.MapToDtoList(result);

            return list;
        }


        public async Task<GenreDto> GetGenreByIdAsync(Guid id)
        {
            var genre = await _repo.GetGenreByIdAsync(id);
            if (genre == null)
                throw new NotFoundException("жанр не знайдено");

            return GenreMapper.ToGenreDto(genre);
        }


        public async Task<bool> RestoreGenreAsync(Guid id)
        {
            var result = _repo.RestoreGenreAsync(id);

            return await result;
        }


        public async Task<GenreDto> UpdateGenreAsync(Guid id, GenreDto dto)
        {
            var currentGenre = await _repo.GetGenreByIdAsync(id);

            if (currentGenre == null)
                throw new NotFoundException("жанр не знайдено");

            GenreMapper.UpdateData(currentGenre, dto);

            var result = await _repo.UpdateGenreAsync(currentGenre);

            return GenreMapper.ToGenreDto(result);
        }
    }
}
