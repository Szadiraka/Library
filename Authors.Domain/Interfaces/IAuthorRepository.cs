using Authors.Domain.Domains;
using Authors.Domain.Queries;

namespace Authors.Domain.Interfaces
{
    public  interface IAuthorRepository
    {
        Task<Author> GetByIdAsync(Guid id);

        Task<PagedResult<Author>> GetAllAsync(AuthorQuery query);

        Task<bool> ExistsAsync(string fullName, DateOnly? birthDate);

        Task<Author> AddAsync(Author author);

        Task<Author> UpdateAsync(Author author );

        Task DeleteAsync(Guid id );

        Task<List<Author>> GetAllAuthorsByIdsAsync(List<Guid> ids);

        
    }
}
