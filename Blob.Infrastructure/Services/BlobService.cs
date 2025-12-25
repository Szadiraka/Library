
using Blob.Application.Dtos;
using Blob.Application.Exceptions;
using Blob.Application.Interfaces;
using Blob.Domain.Entities;
using Minio;
using Minio.DataModel.Args;

namespace Blob.Infrastructure.Services
{
    public class BlobService : IBlobService
    {

        private readonly IMinioClient _minio;

        public BlobService(IMinioClient minio)
        {
            _minio = minio;

        }

        public async Task AddFileAsync(FileMetaData file)
        {
            bool exists = await _minio.BucketExistsAsync(
                new BucketExistsArgs().WithBucket(file.Bucket!.Name));


            using (file.Stream)
            {
                await _minio.PutObjectAsync(new PutObjectArgs()
               .WithBucket(file.Bucket.Name)
               .WithObject(file.StorageName)
               .WithStreamData(file.Stream)
               .WithObjectSize(file.Size)
               .WithContentType(file.ContentType)
                );
            }



        }

        public async Task DeleteFileAsync(string containerName, string fileName)
        {
            bool exists = await _minio.BucketExistsAsync(
                new BucketExistsArgs().WithBucket(containerName));

            if (!exists)
                throw new NotFoundException("Bucket не знайдено");

            try
            {
                await _minio.StatObjectAsync(new StatObjectArgs()
                              .WithBucket(containerName)
                              .WithObject(fileName));

            }
            catch
            {
                throw new NotFoundException("Файл в зазначеному бакет не знайдено");
            }



            await _minio.RemoveObjectAsync(
                   new RemoveObjectArgs()
                   .WithBucket(containerName)
                   .WithObject(fileName));
            return;

        }

        //public async Task<List<string>> GetAllFilesAsync(string containerName)
        //{
        //    var result = new List<string>();

        //    var objects = _minio.ListObjectsEnumAsync(
        //        new ListObjectsArgs()
        //       .WithBucket(containerName)
        //       .WithRecursive(true));

        //    await foreach (var item in objects)
        //    {
        //        result.Add(item.Key);
        //    }
        //    return result;
        //}

        public async Task<MemoryStream> GetFileAsync(string containerName, string fileName)
        {
            bool exists = await _minio.BucketExistsAsync(
               new BucketExistsArgs().WithBucket(containerName));

            if (!exists)
                throw new NotFoundException("Bucket не знайдено");


            var ms = new MemoryStream();

            await _minio.GetObjectAsync(new GetObjectArgs()
            .WithBucket(containerName)
            .WithObject(fileName)
            .WithCallbackStream(stream => stream.CopyTo(ms)));

            ms.Position = 0;


            //var stat = await _minio.StatObjectAsync(
            //    new StatObjectArgs()
            //    .WithBucket(containerName)
            //    .WithObject(fileName)
            //  );

            return ms;

            //return (ms, stat.ContentType);
        }
      
        //public async Task RenameFileAsync(string containerName, string fileName, string newFileName)
        //{
        //    await _minio.CopyObjectAsync(
        //        new CopyObjectArgs()
        //        .WithBucket(containerName)
        //        .WithObject(newFileName)
        //        .WithCopyObjectSource(
        //            new CopySourceObjectArgs()
        //            .WithBucket(containerName)
        //            .WithObject(fileName))
        //     );

        //    await _minio.RemoveObjectAsync(
        //        new RemoveObjectArgs()
        //        .WithBucket(containerName)
        //        .WithObject(fileName)

        //     );
        //}

        //-----------------------------------------------------------------------------
       
        public async Task RemoveBucketAsync(string containerName)
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

            if (result.Count > 0)
                throw new ConflictException("Bucket не порожній, ви не можете його видалити");

            await _minio.RemoveBucketAsync(new RemoveBucketArgs()
                .WithBucket(containerName));

        }

        public async Task AddBucketAsync(string bucketName)
        {
            bool exists = await _minio.BucketExistsAsync(
              new BucketExistsArgs().WithBucket(bucketName));
            if (exists)
                throw new ConflictException("Bucket вже існує");

            await _minio.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));             
            
        }

    }
}
