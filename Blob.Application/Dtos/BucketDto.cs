
using System.ComponentModel.DataAnnotations;

namespace Blob.Application.Dtos
{
    public class BucketDto
    {
        public Guid? Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public DateTimeOffset CreatedAt { get; set; } = new DateTimeOffset(DateTime.UtcNow);
      
        public DateTimeOffset? UpdatedAt { get; set; }

       
    }
}
