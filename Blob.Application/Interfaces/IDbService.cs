

using Blob.Application.Dtos;
using Blob.Domain.Entities;
using Blob.Domain.Queries;

namespace Blob.Application.Interfaces
{
    
    public interface IDbService
    {

        Task<bool> ExistBucket(Guid bucketId);

        Task<bool> ExistBucket(string name);

        Task DeleteBucket(Bucket bucket);

        Task CreateBucket(Bucket bucket);

        Task  UpdateBucket(Guid bucketId, string newName);

        Task<Bucket> GetBucketById(Guid bucketId);

        Task<List<Bucket>> GetBuckets(BucketQuery query);

        Task<bool> BucketIsEmpty(Guid bucketId);

        //--------------------------------------

        Task DeleteFile(Guid id);        

        Task CreateFile(FileMetaData file);

        Task UpdateFile(Guid id, string newName);

        Task<FileMetaData> GetFileById(Guid id);

        Task<PagedResult<FileMetaData>> GetFiles(FileQuery filter);

        Task MakeActiveFile(Guid id);

        Task<List<FileMetaData>> GetAllFilesByName(string name);

    }
}
