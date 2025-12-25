

namespace Blob.Domain.Queries
{
    public  class FileQuery
    {
        public string? OriginalName { get; set; }

        public Guid? UserId { get; set; }

        public Guid? BookId { get; set; }

        public int? Version { get; set; }

        public bool? IsActive { get; set; }


        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
