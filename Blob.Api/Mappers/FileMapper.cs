
using Blob.Api.Requests;
using Blob.Domain.Entities;

namespace Blob.Api.Mappers
{
    public static class FileMapper
    {
        public static async Task<FileMetaData> ToDto(FileRequest source)
        {
            if (source.File == null)
            {
                throw new ArgumentNullException(nameof(source.File), "File cannot be null.");
            }

            var memoryStream = new MemoryStream();
            await source.File.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            return new FileMetaData()
            {
                OriginalName = source.OriginalName ?? source.File.FileName,
                StorageName = Guid.NewGuid().ToString(),
                Size = source.File.Length,
                ContentType = source.File.ContentType,
                Stream = memoryStream,
                BookId = source.BookId,
                BucketId = source.BucketId,
                Version = source.Version ?? 0,  
                CreatedAt = DateTimeOffset.UtcNow,               
               
            };
        }

      
    }
}
