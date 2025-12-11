

namespace Authors.Application.DTOs
{
    public  class AuthorIdsRequest
    {
        public List<Guid> Ids { get; set; } = new List<Guid>();
    }
}
