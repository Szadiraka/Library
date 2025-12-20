
using Blob.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Blob.Infrastructure.Persistance
{
    public class AppDbContext: DbContext
    {

        public DbSet<Bucket> Buckets { get; set; }

        public DbSet<FileMetaData> Files { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

    }
}
