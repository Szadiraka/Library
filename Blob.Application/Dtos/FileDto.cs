

namespace Blob.Application.Dtos
{
    public class FileDto
    {
        public string BucketName { get; set; } = string.Empty;

        public string? FileName { get; set; }

        public Stream? File { get; set; } 
    }
}
