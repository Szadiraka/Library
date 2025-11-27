using BooksService.Domain.Entities;
using BooksService.Domain.Interfaces;
using BooksService.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;


namespace BooksService.Infrastructure.Services
{
    public class BookAuthorRepository : IBookAuthorRepository
    {
        private readonly AppDbContext _context;

        public BookAuthorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddBookAuthorAsync(BookAuthor entity)
        {           
            await _context.BookAuthors.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteBookAuthorAsync(BookAuthor entity)
        {
            _context.BookAuthors.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistBookAuthorAsync(BookAuthor entity)
        {
            var result = await _context.BookAuthors.AnyAsync(x => x.AuthorId == entity.AuthorId &&
                          x.BookId == entity.BookId);
            return result;
        }

        public Task<List<Guid>> GetAuthorsIdsByBookIdAsync(Guid bookId)
        {
           var result = _context.BookAuthors.Where(x=>x.BookId == bookId)
                .Select(x => x.AuthorId).ToListAsync();
            return result;

        }
    }
}
