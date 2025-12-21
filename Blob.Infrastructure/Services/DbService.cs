using Blob.Application.Exceptions;
using Blob.Application.Interfaces;
using Blob.Domain.Queries;
using Blob.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
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


    }
}
