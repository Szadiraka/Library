
using Blob.Application.Dtos;
using Blob.Application.Interfaces;
using Minio;
using Minio.ApiEndpoints;
using Minio.DataModel.Args;

namespace Blob.Infrastructure.Services
{
    public class BlobService: IBlobService
    {

        private readonly IMinioClient _minio;

        public BlobService(IMinioClient minio)
        {
            _minio = minio;

        }

        public async Task AddFileAsync(FileDto fileDto)
        {
           bool exists = await _minio.BucketExistsAsync(new BucketExistsArgs());

            await _minio.PutObjectAsync(new PutObjectArgs()
            .WithBucket(fileDto.BucketName)
            .WithObject(fileDto.FileName)
            .WithStreamData(fileDto.File)
            .WithObjectSize(fileDto.File!.Length)
            );             

            
        }

        public async Task DeleteFileAsync(string containerName, string fileName)
        {
          
             await _minio.RemoveObjectAsync(
                    new RemoveObjectArgs()
                    .WithBucket(containerName)
                    .WithObject(fileName));
             return ;      
        
        }

        public async Task<List<string>> GetAllFilesAsync(string containerName)
        {
            var result = new List<string>();

            var objects =  _minio.ListObjectsEnumAsync(
                new ListObjectsArgs()
               .WithBucket(containerName)
               .WithRecursive(true));

            await foreach (var item in objects)
            {
                result.Add(item.Key);
            }
            return result;
        }

        public async Task<Stream> GetFileAsync(string containerName, string fileName)
        {
           var ms = new MemoryStream();

            await _minio.GetObjectAsync(new GetObjectArgs()
            .WithBucket(containerName)
            .WithObject(fileName)
            .WithCallbackStream(stream => stream.CopyTo(ms)));

            ms.Position = 0;

            return ms;
        }

        public async Task RenameFileAsync(string containerName, string fileName, string newFileName)
        {
            await _minio.CopyObjectAsync(
                new CopyObjectArgs()
                .WithBucket(containerName)
                .WithObject(newFileName)
                .WithCopyObjectSource(
                    new CopySourceObjectArgs()
                    .WithBucket(containerName)
                    .WithObject(newFileName))
             );

            await _minio.RemoveObjectAsync(
                new RemoveObjectArgs()
                .WithBucket(containerName)
                .WithObject(fileName)
                
             );
        }



    }
}
