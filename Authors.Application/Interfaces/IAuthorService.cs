
using  Authors.Application.DTOs;
using Authors.Domain.Interfaces;
using Authors.Domain.Queries;

namespace Authors.Application.Interfaces
{
    public  interface IAuthorService
    {
        Task<AuthorDto> GetByIdAsync(Guid id);

        Task<PagedResult<AuthorDto>> GetAllAsync(AuthorQuery authorQuery);

        Task<bool> ExistsAsync(string fullName, DateOnly? birthDate);

        Task<AuthorDto> AddAsync( AuthorDto author);

        Task<AuthorDto> UpdateAsync(Guid id,AuthorDto author);

        Task DeleteAsync(Guid id);

        Task<List<AuthorDto>> GetAllByIdsAsync(List<Guid> ids);



    }
}
