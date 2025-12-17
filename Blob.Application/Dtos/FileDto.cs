

namespace Blob.Application.Dtos
{
    public class FileDto
    {
        public string BucketName { get; set; } = string.Empty;

        public string? FileName { get; set; }

        public Stream? Stream { get; set; } 

        public string ContentType { get; set; } = string.Empty;

        public long Length { get; set; }
    }
}
