using Authors.Application.DTOs;
using Authors.Domain.Domains;
using Authors.Domain.Interfaces;


namespace Authors.Application.Mappers
{
    public static class AuthorMapper
    {

        public static Author ToAuthor(AuthorDto dto)
        {
            return new Author
            {
                Id = Guid.NewGuid(),
                FullName = dto.FullName,
                Bio = dto.Bio,
                BirthDate = dto.BirthDate,
                DeathDate = dto.DeathDate,
                Nationality = dto.Nationality,
                CreatedAt = DateTimeOffset.UtcNow
            };
        }

        public static AuthorDto ToAuthorDto(Author author)
        {
            return new AuthorDto
            {
                Id = author.Id,
                FullName = author.FullName,
                Bio = author.Bio,
                BirthDate = author.BirthDate,
                DeathDate = author.DeathDate,
                Nationality = author.Nationality
            };
        }  
        
        public static PagedResult<AuthorDto> MapToDtoList(PagedResult<Author> entities)
        {
            return new PagedResult<AuthorDto>
            {
                Page = entities.Page,
                PageSize = entities.PageSize,
                Items = entities.Items.Select(x => ToAuthorDto(x)).ToList(),
                TotalCount = entities.TotalCount
            };
           
        }
           
        public static void  UpdateData(Author entiry, AuthorDto dto)
        {
            entiry.FullName = dto.FullName;
            entiry.Bio = dto.Bio;
            entiry.BirthDate = dto.BirthDate;
            entiry.DeathDate = dto.DeathDate;
            entiry.Nationality = dto.Nationality;
            entiry.UpdatedAt = DateTimeOffset.UtcNow;               
        }
       
    }    

 }
     
    

