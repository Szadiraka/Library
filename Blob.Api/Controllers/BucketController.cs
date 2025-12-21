using Blob.Api.Mappers;
using Blob.Application.Dtos;
using Blob.Application.Exceptions;
using Blob.Application.Interfaces;
using Blob.Domain.Entities;
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
            Bucket result = await _service.GetBucketByIdAsync(id);
            BucketDto bucketDto = BucketMapper.ToDto(result);
            return Ok(new ApiResponse<BucketDto> { Message = "Backet отримано", Data = bucketDto });
        }


        [HttpGet("list")]
        public async Task<IActionResult> GetBuckets([FromQuery] BucketQuery query)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object> { Message = "Не коректний запит" });

            var buckets = await _service.GetAllBucketsAsync(query);

            List<BucketDto> dtos = BucketMapper.ToDtoList(buckets);

            return Ok(new ApiResponse<List<BucketDto>> { Message = "Buckets отримано", Data = dtos });
        }

        [HttpPut("rename/{id:guid}")]
        public async Task<IActionResult> RenameBucketName(Guid id, [FromQuery] string newName)
        {
            await _service.RenameBucketNameAsync(id, newName);
            return Ok(new ApiResponse<object> { Message = "Назва змінена" });
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddBucket ([FromBody] BucketDto bucketDto)
        {
            if(!ModelState.IsValid)
                throw new _ValidationException("Невалідні дані");

            Bucket bucket = BucketMapper.ToEntity(bucketDto);
            bucket.StorageName = bucketDto.Name;
            await _service.AddBucketAsync(bucket);
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
