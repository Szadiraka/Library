

using BooksService.Application.DTOs;
using BooksService.Domain.Entities;
using BooksService.Domain.Interfaces;

namespace BooksService.Application.Mappers
{
    public static class GenreMapper
    {
        public static Genre ToGenre(GenreDto dto)
        {
            return new Genre
            {
                Name = dto.Name,           
                IsDeleted = dto.IsDeleted,           
                CreatedAt = DateTimeOffset.UtcNow,

            };
        }

        public static GenreDto ToGenreDto(Genre genre)
        {
            return new GenreDto
            {
                Id = genre.Id,
                Name = genre.Name,            
                IsDeleted = genre.IsDeleted                
            };
        }

        public static PagedResult<GenreDto> MapToDtoList(PagedResult<Genre> entities)
        {
            return new PagedResult<GenreDto>
            {
                Page = entities.Page,
                PageSize = entities.PageSize,
                Items = entities.Items.Select(x => ToGenreDto(x)).ToList(),
                TotalCount = entities.TotalCount
            };

        }

        public static void UpdateData(Genre entity, GenreDto dto)
        {
            entity.Name = dto.Name;          
            entity.IsDeleted = dto.IsDeleted;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

        }

        public static List<GenreDto> MatToSimpleListDto(List<Genre> entities)
        {
            return entities.Select(x => ToGenreDto(x)).ToList();
        }
    }
}
