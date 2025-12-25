
using Blob.Application.Dtos;
using Blob.Domain.Entities;
using Blob.Domain.Queries;

namespace Blob.Application.Interfaces
{
    public interface IFileMetaDataService
    {
        public Task<Bucket> GetBucketByIdAsync(Guid id);

        public Task<List<Bucket>> GetAllBucketsAsync(BucketQuery query);

        public Task RenameBucketNameAsync(Guid id, string newName);

        public Task AddBucketAsync(Bucket bucket);

        public Task DeleteBucketAsync(Guid id);

        //---------------------------------------

        public Task<FileMetaData> FileMetaDataByIdAsync(Guid id);

        //public Task<MemoryStream> GetFileByIdAsync(Guid id);

        public Task<(MemoryStream, string, string)> GetFileByIdAsync(Guid id);

        public Task<PagedResult<FileMetaData>> GetAllFileMetadatasAsync(FileQuery query);

        public Task RenameFileAsync(Guid id, string newName, bool exceptionIfExist);

        public Task AddFileAsync(FileMetaData file);
      

        public Task DeleteFileAsync(Guid id);

    }
}
