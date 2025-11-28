

using System.Text.Json.Serialization;

namespace BooksService.Domain.Entities
{
    public class Genre
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string .Empty;       

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }


        [JsonIgnore]
        public List<Book> Books { get; set; } = new List<Book>();
    }
}
