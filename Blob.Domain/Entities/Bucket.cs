

namespace Blob.Domain.Entities
{
    public  class Bucket
    {
        public Guid Id { get; set; }
       
        public string Name { get; set; } = string.Empty;

        public string StorageName { get; set; } = string.Empty;

        public DateTimeOffset CreatedAt { get; set; } 

        public DateTimeOffset? UpdatedAt { get; set; }

        public List<FileMetaData> Files { get; set; } = new();
    }
}
