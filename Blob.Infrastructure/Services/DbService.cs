using Blob.Application.Dtos;
using Blob.Application.Exceptions;
using Blob.Application.Interfaces;
using Blob.Domain.Entities;
using Blob.Domain.Queries;
using Blob.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Minio.DataModel.Notification;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Bucket = Blob.Domain.Entities.Bucket;

namespace Blob.Infrastructure.Services
{
    public class DbService: IDbService
    {
        private readonly  AppDbContext _context;

        public DbService(AppDbContext context)
        {
            _context = context;
        }
    

        public async Task CreateBucket(Bucket bucket)
        {
            await _context.Buckets.AddAsync(bucket);
            await _context.SaveChangesAsync();
           
        }       

        public async Task DeleteBucket(Bucket bucket)
        {
            _context.Buckets.Remove(bucket);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistBucket(Guid bucketId)
        {
            return await _context.Buckets.AnyAsync(x => x.Id == bucketId);
        }

        public async Task<bool> ExistBucket(string name)
        {         
            return await _context.Buckets
                 .AnyAsync(x => x.Name.ToUpper()== name.ToUpper());

        }

        public async Task<Bucket> GetBucketById(Guid bucketId)
        {
            var bucket = await _context.Buckets.FirstOrDefaultAsync(x => x.Id == bucketId);
            if (bucket == null)
                throw new NotFoundException("Bucket не знайдено");
            return bucket;
        }

        public async Task<List<Bucket>> GetBuckets(BucketQuery bucketQuery)
        {       

            var query = _context.Buckets.AsQueryable();

             if(bucketQuery.Title != null)
                query = query.Where(x => x.Name.ToUpper().Contains(bucketQuery.Title.ToUpper()));

             if(bucketQuery.CreatedAfter != null)
                query = query.Where(x => x.CreatedAt >= bucketQuery.CreatedAfter);

             if (bucketQuery.CreatedBefore != null)
                query = query.Where(x => x.CreatedAt <= bucketQuery.CreatedBefore);

             return await query.ToListAsync();
           
        }

        public async Task UpdateBucket(Guid bucketId, string newName)
        {
            var bucket = await _context.Buckets.FirstOrDefaultAsync(x => x.Id == bucketId);
            if (bucket == null)
                throw new NotFoundException("Bucket не знайдено");

            bucket.Name = newName;
            bucket.UpdatedAt = DateTimeOffset.Now;
            _context.Buckets.Update(bucket);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> BucketIsEmpty(Guid bucketId)
        {
            return !await _context.Files.AnyAsync(x=>x.BucketId == bucketId);
        }

        //-------------------------------------------

        public async Task DeleteFile(Guid id)
        {
            FileMetaData? file = await _context.Files.FirstOrDefaultAsync(x => x.Id == id);
            if (file == null)
                throw new NotFoundException("Файл не знайдено");
            
            _context.Files.Remove(file);
            await _context.SaveChangesAsync();
        }

        public async Task CreateFile(FileMetaData file)
        {
           
            if (file.Version > 0)
            {
                var query = _context.Files
                     .Where(x => x.OriginalName.ToUpper() == file.OriginalName.ToUpper());

                int maxVersion = await query.AnyAsync() ? await query.MaxAsync(x => x.Version) : 0;

                file.Version = maxVersion + 1;
            }
            
            await _context.Files.AddAsync(file);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFile(Guid id, string newName)
        {
          var file = await _context.Files.FirstOrDefaultAsync(x=>x.Id == id);
            if(file == null)
                throw new NotFoundException("Файл не знайдено");

            file.UpdatedAt = DateTimeOffset.Now;
            file.OriginalName = newName;

             _context.Files.Update(file);
            await _context.SaveChangesAsync();

        }

        public async Task MakeActiveFile(Guid id)
        {
            var file = await _context.Files.FirstOrDefaultAsync(x => x.Id == id);
            if (file == null)
                throw new NotFoundException("Файл не знайдено");

            var query = await _context.Files.Where(x=>x.OriginalName.ToUpper() == file.OriginalName
                            .ToUpper() && x.Id !=id).ToListAsync();

            foreach (var item in query)
            {
                item.IsActive = false;
                item.UpdatedAt = DateTimeOffset.Now;
            }
            file.IsActive = true;
            file.UpdatedAt = DateTimeOffset.Now;
            await _context.SaveChangesAsync();
        }

        public async Task<FileMetaData> GetFileById(Guid id)
        {
            var file = await _context.Files.FirstOrDefaultAsync(x => x.Id == id);
            if (file == null)
                throw new NotFoundException("Файл не знайдено");

            return file;
        }

        public async Task<PagedResult<FileMetaData>> GetFiles(FileQuery filter)
        {

            var query = _context.Files.AsQueryable();

            if (filter.OriginalName != null)
                query = query.Where(x => x.OriginalName.ToUpper().Contains(filter.OriginalName.ToUpper()));

            if (filter.UserId != null)
                query = query.Where(x => x.UserId == filter.UserId);

            if (filter.BookId != null)
                query = query.Where(x => x.UserId == filter.UserId);

            if(filter.Version != null)
                query = query.Where(x => x.Version == filter.Version);


            if (filter.IsActive != null)
                query = query.Where(x => x.IsActive == filter.IsActive);

            var total = await query.CountAsync();

            var result = await query
                   .OrderBy(x => x.OriginalName)
                   .Skip((filter.Page - 1) * filter.PageSize)
                   .Take(filter.PageSize)
                   .ToListAsync();

            return new PagedResult<FileMetaData>
            {
                Items = result,
                TotalCount = total,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }

        public async Task<List<FileMetaData>> GetAllFilesByName(string name)
        {
            return await _context.Files.Where(x => x.OriginalName.ToUpper() == name.ToUpper()).ToListAsync();
        }





    }
}
