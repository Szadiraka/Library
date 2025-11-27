using BooksService.Application.DTOs;
using BooksService.Application.Exceptions;
using BooksService.Application.Interfaces;
using BooksService.Application.Mappers;
using BooksService.Domain.Interfaces;
using System.Net.WebSockets;


namespace BooksService.Application.Services
{
    public class BookAuthorService: IBookAuthorService
    {

        private readonly IBookAuthorRepository _repo;

        public BookAuthorService(IBookAuthorRepository repo)
        {
            _repo = repo;
        }


        public async Task<bool> AddBookAuthorAsync(BookAuthorDto dto)
        {
            var exists = await _repo.ExistBookAuthorAsync(BookAuthorMapper.ToBookAuthor(dto));
            if (exists)
                throw new BusinessRuleException("така книга-автор існує");

            var result = await _repo.AddBookAuthorAsync(BookAuthorMapper.ToBookAuthor(dto));

            return result;
        }

        public async Task DeleteBookAuthorAsync(BookAuthorDto dto)
        {
            var exists = await _repo.ExistBookAuthorAsync(BookAuthorMapper.ToBookAuthor(dto));
            if (!exists)
                throw new BusinessRuleException("такої книга-автор не існує");
            await _repo.DeleteBookAuthorAsync(BookAuthorMapper.ToBookAuthor(dto));
        }

        public async Task<List<Guid>> GetAuthorsIdsByBookIdAsync(Guid bookId)
        {
            var result = await _repo.GetAuthorsIdsByBookIdAsync(bookId);
            return result;
        }
    }
}
