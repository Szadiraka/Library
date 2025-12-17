
using Blob.Api.Requests;
using Blob.Application.Dtos;

namespace Blob.Api.Mappers
{
    public static class FileMapper
    {
        public static async Task<FileDto> ToDto(FileRequest source)
        {
            if (source.File == null)
            {
                throw new ArgumentNullException(nameof(source.File), "File cannot be null.");
            }

            var memoryStream = new MemoryStream();
            await source.File.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            return new FileDto()
            {
                FileName = source.FileName,
                BucketName = source.BucketName,
                Stream = memoryStream,
                ContentType = source.File.ContentType,
                Length = source.File.Length
            };
        }
    }
}
