
using Blob.Application.Dtos;
using Blob.Domain.Entities;
using Blob.Domain.Queries;

namespace Blob.Application.Interfaces
{
    public interface IFileMetaDataService
    {
        public Task<BucketDto> GetBucketByIdAsync(Guid id);

        public Task<List<BucketDto>> GetAllBucketsAsync(BucketQuery query);

        public Task RenameBucketNameAsync(Guid id, string newName);

        public Task AddBucketAsync(Bucket bucket);

        public Task DeleteBucketAsync(Guid id);

    }
}
