
namespace Blob.Domain.Queries
{
    public class BucketQuery
    {
        public string? Title { get; set; }

        public DateTimeOffset? CreatedAfter { get; set; }

        public DateTimeOffset? CreatedBefore { get; set; }
      
    }
}
