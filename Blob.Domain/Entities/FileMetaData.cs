

namespace Blob.Domain.Entities
{
    public class FileMetaData
    {

        public Guid Id { get; set; }

        public string OriginalName { get; set; } = string.Empty;

        public string StorageName { get; set; } = string.Empty; 

        public long Size { get; set; }

        public string ContentType { get; set; } = string.Empty;

        public Guid? UserId { get; set; } 

        public Guid? BookId { get; set; }

        public int Version { get; set; }

        public bool IsActive { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }



        public Guid BucketId { get; set; }

        public Bucket? Bucket { get; set; }
    }
}
