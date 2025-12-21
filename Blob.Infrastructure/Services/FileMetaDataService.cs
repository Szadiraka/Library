
using Blob.Application.Dtos;
using Blob.Application.Exceptions;
using Blob.Application.Interfaces;
using Blob.Domain.Queries;
using Bucket = Blob.Domain.Entities.Bucket;


namespace Blob.Infrastructure.Services
{
    public class FileMetaDataService : IFileMetaDataService
    {
        private readonly IDbService _dbService;
        private readonly IBlobService _blobService;

        public FileMetaDataService(IDbService dbService, IBlobService blobService)
        {
            _dbService = dbService;
            _blobService = blobService;
        }

        public async Task AddBucketAsync(Bucket bucket)
        {        
            if(await _dbService.ExistBucket(bucket.Name))
                throw new BusinessRuleException("Бакет с такою назвою вже існує");

            await _blobService.AddBucketAsync(bucket.Name);

            await _dbService.CreateBucket(bucket);
        }

        public async Task DeleteBucketAsync(Guid id)
        {
            Bucket bucket = await _dbService.GetBucketById(id);              

            if( !await _dbService.BucketIsEmpty(id))
                throw new BusinessRuleException("Бакет не порожній, ви не можете його видалити");       

             await _blobService.RemoveBucketAsync(bucket.StorageName);

             await _dbService.DeleteBucket(bucket);
           
        }

        public async Task<List<Bucket>> GetAllBucketsAsync(BucketQuery query)
        {
            var result = await _dbService.GetBuckets(query);          

            return result;
        }

        public async Task<Bucket> GetBucketByIdAsync(Guid id)
        {
            Bucket bucket = await _dbService.GetBucketById(id);             

           return bucket;
           
        }

        public async Task RenameBucketNameAsync(Guid id, string newName)
        {        
            await _dbService.UpdateBucket(id,newName);     

        }
    }
}
