

namespace Blob.Application.Dtos
{
    public class ApiResponse<T> where T : new()
    {

        public string? Message {get; set;} 

        public T? Data { get; set; }
    }
}
