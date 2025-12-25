
using Blob.Application.Dtos;
using Blob.Application.Exceptions;
using Blob.Application.Interfaces;
using Blob.Domain.Entities;
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

        public async Task RenameBucketNameAsync(Guid id, string newName)
        {        
            await _dbService.UpdateBucket(id,newName);     

        }

        public async Task DeleteBucketAsync(Guid id)
        {
            Bucket bucket = await _dbService.GetBucketById(id);

            if (!await _dbService.BucketIsEmpty(id))
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

        //----------------------------------------------------------------------

        public async Task AddFileAsync(FileMetaData file)
        {
            Bucket bucket = await _dbService.GetBucketById(file.BucketId);
            file.Bucket = bucket;
            await _blobService.AddFileAsync(file);

            await _dbService.CreateFile(file);
        }

        public async Task DeleteFileAsync(Guid id)
        {
            var file = await _dbService.GetFileById(id);
            var bucket = await _dbService.GetBucketById(file.BucketId);

            await _blobService.DeleteFileAsync(bucket.Name, file.StorageName);

            await _dbService.DeleteFile(id);
        }

        public async Task<FileMetaData> FileMetaDataByIdAsync(Guid id)
        {
            var file = await _dbService.GetFileById(id);
            return file;
        }     

        public async Task<PagedResult<FileMetaData>> GetAllFileMetadatasAsync(FileQuery query)
        {
            var result = await _dbService.GetFiles(query);
            return result;
        }

        public async Task<(MemoryStream, string, string)> GetFileByIdAsync(Guid id)
        {
            var file = await _dbService.GetFileById(id);

            var bucket = await _dbService.GetBucketById(file.BucketId);

            if (bucket == null || file == null)
                throw new NotFoundException("Файл, або бакет не знайдено");

            var ms = await _blobService.GetFileAsync(bucket.Name, file.StorageName);

            return (ms, file.ContentType, file.OriginalName);
        }

        public async Task RenameFileAsync(Guid id, string newName, bool exceptionIfExist = false)
        {
           if (exceptionIfExist && (await _dbService.GetAllFilesByName(newName)).Count > 0)
                 throw new BusinessRuleException("файли з такою назвою вже існують");

            await _dbService.UpdateFile(id, newName);               
        }

     
    }
}
