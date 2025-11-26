using Authors.Domain.Domains;
using Microsoft.EntityFrameworkCore;


namespace Authors.Infrastructure.Persistance
{
    public class AppDbContext: DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Author> Authors { get; set; }
    }
}
