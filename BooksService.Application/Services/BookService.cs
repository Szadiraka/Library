using BooksService.Application.DTOs;
using BooksService.Application.Exceptions;
using BooksService.Application.Interfaces;
using BooksService.Application.Mappers;
using BooksService.Domain.Entities;
using BooksService.Domain.Interfaces;
using BooksService.Domain.Queries;

namespace BooksService.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repo;

        public BookService(IBookRepository repo)
        {
            _repo = repo;
        }


        public async Task<BookDto> AddBookAsync(BookDto dto)
        {
            var exists = await _repo.ExistsBookAsync(dto.Title, dto.PublishedYear);
            if (exists)
                throw new BusinessRuleException("така кника вже існує");

            var result = await _repo.AddBookAsync(BookMapper.ToBook(dto));

            return BookMapper.ToBookDto(result);
        }

        public async Task DeleteBookAsync(Guid id)
        {
            await _repo.DeleteBookAsync(id);
        }

        public async Task<bool> ExistsBookAsync(string title, int PublishedYear)
        {
            return await _repo.ExistsBookAsync(title, PublishedYear);
        }

        public async Task<PagedResult<BookDto>> GetAllBooksByAuthorIdAsync(BookAuthorQuery query)
        {
           
           var result = await  _repo.GetAllBooksByAuthorIdAsync(query);
            return BookMapper.MapToDtoList(result);
        }

        public async Task<PagedResult<BookDto>> GetAllBooksAsync(BookQuery query)
        {
            PagedResult<Book> result = await _repo.GetAllBooksAsync(query);

            var list = BookMapper.MapToDtoList(result);

            return list;
        }

        public async Task<List<BookDto>> GetAllDeletedBooksAsync()
        {
            var result = await _repo.GetAllDeletedBooksAsync();
            return BookMapper.MatToSimpleListDto(result);
        }

        public async Task<BookDto> GetBookByIdAsync(Guid id)
        {
            var book = await _repo.GetBookByIdAsync(id);
            if (book == null)
                throw new NotFoundException("жанр не знайдено");

            return BookMapper.ToBookDto(book);
        }

        public async Task<bool> RestoreBookAsync(Guid id)
        {
            var result = _repo.RestoreBookAsync(id);

            return await result;
        }

        public async Task<BookDto> UpdateBookAsync(Guid id, BookDto book)
        {
            var currentBook = await _repo.GetBookByIdAsync(id);

            if (currentBook == null)
                throw new NotFoundException("книга не знайдено");

            BookMapper.UpdateData(currentBook, book);

            var result = await _repo.UpdateBookAsync(currentBook);

            return BookMapper.ToBookDto(result);
        }

      
    }
}
