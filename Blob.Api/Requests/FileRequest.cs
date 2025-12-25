using System.ComponentModel.DataAnnotations;

namespace Blob.Api.Requests
{
    public class FileRequest
    {
      
            [Required]
            public Guid BucketId { get; set; }

            public string? OriginalName { get; set; }

            [Required]
            public IFormFile? File { get; set; }

            public Guid? BookId { get; set; }

            public int? Version { get; set; }
        
    }
}
