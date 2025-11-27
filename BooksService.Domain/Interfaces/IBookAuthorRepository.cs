using BooksService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksService.Domain.Interfaces
{
    public interface IBookAuthorRepository
    {

        Task<bool> AddBookAuthorAsync(BookAuthor entity);

        Task DeleteBookAuthorAsync(BookAuthor entity);

        Task<List<Guid>> GetAuthorsIdsByBookIdAsync(Guid bookId);

        Task<bool> ExistBookAuthorAsync(BookAuthor entity);
    }
}
