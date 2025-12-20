using Blob.Application.Dtos;
using Blob.Application.Interfaces;
using Blob.Domain.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Blob.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BucketController : ControllerBase
    {
        private readonly IFileMetaDataService _service;

        public BucketController(IFileMetaDataService service )
        {
            _service = service;
        }



        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetBucketById( Guid id)
        {
            var result = await _service.GetBucketByIdAsync(id);
            return Ok(new ApiResponse<BucketDto> { Message = "Backet отримано", Data = result });
        }


        [HttpGet("list")]
        public async Task<IActionResult> GetBuckets([FromQuery] BucketQuery query)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object> { Message = "Не коректний запит" });

            var buckets = await _service.GetAllBucketsAsync(query);
            return Ok(new ApiResponse<List<BucketDto>> { Message = "Buckets отримано", Data = buckets });
        }

        [HttpPut("rename/{id:guid}")]
        public async Task<IActionResult> RenameBucketName(Guid id, [FromQuery] string newName)
        {
            await _service.RenameBucketNameAsync(id, newName);
            return Ok(new ApiResponse<object> { Message = "Назва змінена" });
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddBucket ([FromQuery] string name)
        {
            await _service.AddBucketAsync(name);
            return Ok(new ApiResponse<object> { Message = "bucket додано" });
        }

        [HttpDelete("delete/{id:guid}")]
        public async  Task<IActionResult> DeleteBucket(Guid id)
        {
            await _service.DeleteBucketAsync(id);
            return Ok(new ApiResponse<object> { Message = "Bucket видалено" });
        }










    }
}
