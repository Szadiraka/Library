

using BooksService.Application.Exceptions;
using BooksService.Domain.Entities;
using BooksService.Domain.Interfaces;
using BooksService.Domain.Queries;
using BooksService.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace BooksService.Infrastructure.Services
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Book> AddBookAsync(Book book)
        {
            if (await ExistsBookAsync(book.Title, book.PublishedYear))
                throw new BusinessRuleException("Така книга вже існує");

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task DeleteBookAsync(Guid id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (book == null)
                throw new NotFoundException("Книгу не знайдено");
            if (book.IsDeleted)
                throw new BusinessRuleException("Книга вже видалена");

            book.IsDeleted = true;
            book.DeletedAt = DateTime.UtcNow;
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsBookAsync(string title, int PublishedYear)
        {
            string titleToUpper = title.ToUpper();
            var result = await _context.Books.AnyAsync(x => x.Title.ToUpper() == titleToUpper &&
                               x.PublishedYear == PublishedYear && x.IsDeleted == false);
            return result;
        }

        public async Task<PagedResult<Book>> GetAllBooksAsync(BookQuery query)
        {
            var books = _context.Books.Where(x => x.IsDeleted == false).Include(x=>x.Genre).AsQueryable();

            if (!string.IsNullOrEmpty(query.Title))
            {
                var title = query.Title.ToUpper();
                books = books.Where(x => x.Title.ToUpper().Contains(title));
            }
            if (query.PublishedYear >= 0)
            {
                books = books.Where(x => x.PublishedYear == query.PublishedYear);
            }
            if (query.GenreId != null)
            {
                books = books.Where(x => x.GenreId == query.GenreId);
            }

            var total = await books.CountAsync();

            var result = await books
                   .OrderBy(x => x.Title)
                   .Skip((query.Page - 1) * query.PageSize)
                   .Take(query.PageSize)
                   .ToListAsync();

            return new PagedResult<Book>
            {
                Items = result,
                TotalCount = total,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<PagedResult<Book>> GetAllBooksByAuthorIdAsync(BookAuthorQuery query)
        {
            var books = _context.Books.Include(x=> x.Genre).AsQueryable();
            books = books.Where(x => x.BookAuthors
                   .Any(ba => ba.AuthorId == query.AuthorId));

            var total = await books.CountAsync();

            var result = await books
                  .OrderBy(x => x.Title)
                  .Skip((query.Page - 1) * query.PageSize)
                  .Take(query.PageSize)
                  .ToListAsync();

            return new PagedResult<Book>
            {
                Items = result,
                TotalCount = total,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<PagedResult<Book>> GetAllBooksByGenreIdAsync(GenreIdQuery query)
        {
            var books = _context.Books.Include(x => x.Genre).AsQueryable();
            books = books.Where(x => x.GenreId == query.GenreId);                  

            var total = await books.CountAsync();

            var result = await books
            .OrderBy(x => x.Title)
                  .Skip((query.Page - 1) * query.PageSize)
                  .Take(query.PageSize)
                  .ToListAsync();

            return new PagedResult<Book>
            {
                Items = result,
                TotalCount = total,
                Page = query.Page,
                PageSize = query.PageSize
            };





        }

        public async Task<List<Book>> GetAllDeletedBooksAsync()
        {
            var result = await _context.Books.Where(x => x.IsDeleted == true).Include(x => x.Genre).ToListAsync();
            return result;
        }

        public async Task<Book?> GetBookByIdAsync(Guid id)
        {
            Book? book = await _context.Books.Include(x=> x.Genre).FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
          
            return book;
        }

        public async Task<bool> RestoreBookAsync(Guid id)
        {
            var book = _context.Books.FirstOrDefault(x => x.Id == id);
            if (book == null)
                throw new NotFoundException("Книгу не знайдено");
            if (book.IsDeleted == false)
                throw new BusinessRuleException("Книга не видалена");

            book.IsDeleted = false;
            book.DeletedAt = null;
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Book> UpdateBookAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            return book;
        }
    }
}
