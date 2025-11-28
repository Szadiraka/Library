using BooksService.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace BooksService.Infrastructure.Persistance
{
    public  class AppDbContext: DbContext
    {
           

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<BookAuthor> BookAuthors { get; set; }

        public DbSet<Genre> Genres { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BookAuthor>()
                .HasKey(ba=> new { ba.AuthorId, ba.BookId });

            modelBuilder.Entity<BookAuthor>()
           .HasOne(ba => ba.Book)
           .WithMany(b => b.BookAuthors)
           .HasForeignKey(ba => ba.BookId);
        }
    }
}
