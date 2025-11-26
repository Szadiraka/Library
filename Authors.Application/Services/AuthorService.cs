using Authors.Application.DTOs;
using Authors.Application.Exceptions;
using Authors.Application.Interfaces;
using Authors.Application.Mappers;
using Authors.Domain.Domains;
using Authors.Domain.Interfaces;
using Authors.Domain.Queries;


namespace Authors.Application.Services
{
   

    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _repo;

        public AuthorService(IAuthorRepository repo)
        {
            _repo = repo;
        }



        public async  Task<AuthorDto> GetByIdAsync(Guid id)
        {
           var author = await  _repo.GetByIdAsync(id);
            if (author == null)
                throw new NotFoundException("автора не знайдено");

            return AuthorMapper.ToAuthorDto(author);
        }

        public async Task<PagedResult<AuthorDto>> GetAllAsync(AuthorQuery authorQuery)
        {
            PagedResult<Author> result =await _repo.GetAllAsync(authorQuery);

            var list = AuthorMapper.MapToDtoList(result);

            return list;
        }


        public async Task<bool> ExistsAsync(string fullName, DateOnly? birthDate)
        {
            return await _repo.ExistsAsync(fullName, birthDate);
        }


        public async Task<AuthorDto> AddAsync(AuthorDto author)
        {
           var exists = await _repo.ExistsAsync(author.FullName, author.BirthDate);
            if (exists)
                throw new BusinessRuleException("такий автор вже існує");

            var result = await _repo.AddAsync(AuthorMapper.ToAuthor(author));
            return AuthorMapper.ToAuthorDto(result);
        }

        public async Task<AuthorDto> UpdateAsync(Guid id, AuthorDto authorDto)
        {
           var currentAuthor = await _repo.GetByIdAsync(id);

            if (currentAuthor == null)
                throw new NotFoundException("автора не знайдено");

            AuthorMapper.UpdateData(currentAuthor, authorDto);
            
            var result =await _repo.UpdateAsync(currentAuthor);

            return AuthorMapper.ToAuthorDto(result);
        }



        public async Task DeleteAsync(Guid id)
        {    
             await _repo.DeleteAsync(id);
        }


      
      

       
    }
}
