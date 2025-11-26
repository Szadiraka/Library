using Authors.Application.Exceptions;
using Authors.Domain.Domains;
using Authors.Domain.Interfaces;
using Authors.Domain.Queries;
using Authors.Infrastructure.Persistance;

using Microsoft.EntityFrameworkCore;

namespace Authors.Infrastructure.Services
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _context;

        public AuthorRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<Author> AddAsync(Author author)
        {
           await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
            return author;
        }

        public async Task DeleteAsync(Guid id)
        {
            var author = await _context.Authors.FirstOrDefaultAsync(x => x.Id == id);
            if ( author == null)
                throw new NotFoundException("Автор не найден");

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

        }

        public async Task<bool> ExistsAsync(string fullName, DateOnly? birthDate)
        {
           var result =await _context.Authors.AnyAsync(x=>x.FullName == fullName && x.BirthDate == birthDate);
            return result;
        }

        public async Task<PagedResult<Author>> GetAllAsync(AuthorQuery query)
        {
          var authors = _context.Authors.AsQueryable();

            if (!string.IsNullOrEmpty(query.Name))
            {
                var name = query.Name.ToUpper();
                authors = authors.Where(x => x.FullName.ToUpper().Contains(name));
            }
            if(!string.IsNullOrEmpty(query.Nationality))
            {
                var nationality = query.Nationality.ToUpper();
                authors = authors.Where(x => x.Nationality != null &&
               x.Nationality.ToUpper().Contains(nationality));
            }
               

            var total = await authors.CountAsync();
            var result = await authors
                   .OrderBy(x=>x.FullName)
                   .Skip((query.Page -1)*query.PageSize)
                   .Take(query.PageSize)
                   .ToListAsync();

            return new PagedResult<Author>
            {
                Items = result,
                TotalCount = total,
                Page = query.Page,
                PageSize = query.PageSize
            };

        }

        public async Task<Author> GetByIdAsync(Guid id)
        {
            var author =await _context.Authors.FirstOrDefaultAsync(x => x.Id == id);
            if (author == null)
                throw new NotFoundException("Автор не найден");

            return author;
        }

        public async Task<Author> UpdateAsync(Author author)
        {
            _context.Authors.Update(author);
            await  _context.SaveChangesAsync();
            return author;

        }
    }
}
