

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
            //проверить существует ли бакет с таким именем если существует то выбросить исключение
            if(await _dbService.ExistBucket(bucket.Name))
                throw new BusinessRuleException("Бакет с такою назвою вже існує");


            // если такого нет, добавляем в minIO и есил все хорошо то добавляем в БД
            //?????????????????????????


            await _dbService.CreateBucket(bucket);
        }

        public async Task DeleteBucketAsync(Guid id)
        {
            if (!await _dbService.ExistBucket(id))
                throw new NotFoundException("Бакет с такою назвою не існує");
       

            // если все хорошо то удаляем бакет

            //???????????????????

            // удаляем бакет из БД
            await _dbService.DeleteBucket(id);
           
        }

        public async Task<List<BucketDto>> GetAllBucketsAsync(BucketQuery query)
        {
            var result = await _dbService.GetBuckets(query);

            //преобразовать тип данных и вернуть нужный тип

            throw new NotImplementedException();
        }

        public async Task<BucketDto> GetBucketByIdAsync(Guid id)
        {
            if (!await _dbService.ExistBucket(id))
                throw new NotFoundException("Бакет с таким id не існує");

            //получить guid файла и  сделать запрос в minIO

            throw new NotImplementedException();
        }

        public async Task RenameBucketNameAsync(Guid id, string newName)
        {        
            await _dbService.UpdateBucket(id,newName);     

        }
    }
}
