using EmailService.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace EmailService.Infrastructure.Persistance
{
    public class EmailDbContext: DbContext
    {

        public DbSet<EmailMessage> EmailMessages { get; set; }

        public EmailDbContext(DbContextOptions<EmailDbContext> options) : base(options)
        {

        }
    }
}
