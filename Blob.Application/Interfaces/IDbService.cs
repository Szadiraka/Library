

using Blob.Domain.Entities;
using Blob.Domain.Queries;

namespace Blob.Application.Interfaces
{
    
    public interface IDbService
    {

        Task<bool> ExistBucket(Guid bucketId);

        Task<bool> ExistBucket(string name);

        Task DeleteBucket(Guid bucketId);

        Task CreateBucket(Bucket bucket);

        Task  UpdateBucket(Guid bucketId, string newName);

        Task<Bucket> GetBucketById(Guid bucketId);

        Task<List<Bucket>> GetBuckets(BucketQuery query);
    }
}
