using System.ComponentModel.DataAnnotations;

namespace Blob.Api.Requests
{
    public class FileRequest
    {
      
            [Required(ErrorMessage = "BucketName is required")]
            public string BucketName { get; set; } = string.Empty;

            public string? FileName { get; set; }

            [Required(ErrorMessage = "File is required")]
            public IFormFile? File { get; set; }
        
    }
}
