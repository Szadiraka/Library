
using Blob.Application.Dtos;
using Blob.Domain.Entities;

namespace Blob.Application.Interfaces
{
    public  interface IBlobService
    {

        //public Task<List<string>> GetAllFilesAsync(string containerName);

        public Task<MemoryStream> GetFileAsync(string containerName, string fileName);

        public Task AddFileAsync(FileMetaData file);

        public Task DeleteFileAsync(string containerName, string fileName);

        //public Task RenameFileAsync(string containerName, string fileName, string newFileName);

        //-----------------------------------------

        public Task RemoveBucketAsync(string bucketName);

        public Task AddBucketAsync(string bucketName);
    }
}
