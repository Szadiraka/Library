using System.ComponentModel.DataAnnotations;

namespace ApiGateWay.Models
{
    public class BookDto
    {
        public Guid? Id { get; set; }
       
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }
     
        public int PublishedYear { get; set; }

        public Guid GenreId { get; set; }
        public Genre? Genre { get; set; }

        public bool IsDeleted { get; set; }

        //[JsonIgnore]
        //public List<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
    }
}
