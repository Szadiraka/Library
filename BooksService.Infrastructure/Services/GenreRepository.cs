using BooksService.Application.Exceptions;
using BooksService.Domain.Entities;
using BooksService.Domain.Interfaces;
using BooksService.Domain.Queries;
using BooksService.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace BooksService.Infrastructure.Services
{
    public class GenreRepository : IGenreRepository
    {
        private readonly AppDbContext _context;

        public GenreRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Genre> AddGenreAsync(Genre genre)
        {

            if (await ExistsGenreAsync(genre.Name))
                throw new BusinessRuleException("Такий жанр вже існує");

            await _context.Genres.AddAsync(genre);
            await _context.SaveChangesAsync();
            return genre;
        }

        public async Task DeleteGenreAsync(Guid id)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(x => x.Id == id);
            if (genre == null)
                throw new NotFoundException("Жано не знайдено");
            if(genre.IsDeleted)
                throw  new BusinessRuleException("Жанр вже видалений");

            genre.IsDeleted = true;
            genre.DeletedAt = DateTime.UtcNow;
            _context.Genres.Update(genre);
            await _context.SaveChangesAsync();

        }

        public async Task<bool> ExistsGenreAsync(string name)
        {
            string nameToUpper = name.ToUpper();
            var result = await _context.Genres.AnyAsync(x => x.Name.ToUpper() == nameToUpper && x.IsDeleted == false);
            return result;
        }

        public async Task<List<Genre>> GetAllDeletedGenresAsync()
        {
            var result = await _context.Genres.Where(x => x.IsDeleted == true).ToListAsync();
            return result;
        }

        public async Task<PagedResult<Genre>> GetAllGenresAsync(GenreQuery query)
        {
            var genres = _context.Genres.Where(x => x.IsDeleted == false).AsQueryable();

            if (!string.IsNullOrEmpty(query.Name))
            {
                var name = query.Name.ToUpper();
                genres = genres.Where(x => x.Name.ToUpper().Contains(name));
            }
          
            var total = await genres.CountAsync();

            var result = await genres
                   .OrderBy(x => x.Name)
                   .Skip((query.Page - 1) * query.PageSize)
                   .Take(query.PageSize)
                   .ToListAsync();

            return new PagedResult<Genre>
            {
                Items = result,
                TotalCount = total,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<Genre?> GetGenreByIdAsync(Guid id)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);           
            return genre;
        }

        public async Task<bool> RestoreGenreAsync(Guid id)
        {
           var genre = _context.Genres.FirstOrDefault(x => x.Id == id);
            if (genre == null)
                throw new NotFoundException("Жанр не знайдено");
            if(genre.IsDeleted == false)
                throw new BusinessRuleException("Жанр не видалений");

            genre.IsDeleted = false;
            genre.DeletedAt = null;
            _context.Genres.Update(genre);
            await _context.SaveChangesAsync();
            return true;
           
        }

        public async Task<Genre> UpdateGenreAsync(Genre genre)
        {
            _context.Genres.Update(genre);
            await _context.SaveChangesAsync();
            return genre;
        }
    }
}
