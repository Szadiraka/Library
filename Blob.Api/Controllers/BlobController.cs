using Blob.Api.Mappers;
using Blob.Api.Requests;
using Blob.Application.Dtos;
using Blob.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Blob.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobController : ControllerBase
    {
        private readonly IBlobService _service;

        public BlobController(IBlobService service)
        {
            _service = service;
        }

        [HttpGet("download")]
        public  async Task<IActionResult> GetFile([FromQuery] string bucketname, [FromQuery] string filename)
        {
            (MemoryStream? stream,string? contentType) = await  _service.GetFileAsync(bucketname, filename);
            if (stream == null || contentType == null)
              return NotFound(new ApiResponse<object> { Message = "Файл не знайдено, або відсутні дані" });
           
            
            stream.Position = 0;
            return File(stream, contentType, filename);
            

              
        }

       
        [HttpGet("list")]
        public async Task<IActionResult> GetAllFiles([FromQuery] string busketname)
        {         
            var items = await _service.GetAllFilesAsync(busketname);         

            return Ok(new ApiResponse<List<string>> {Message = "Перелік файлів отримано", Data = items });
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile( [FromForm] FileRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object> { Message = "Не валідні дані" });
            }

            var dto = await FileMapper.ToDto(request);

            await _service.AddFileAsync(dto);
           
            return Ok(new ApiResponse<object> { Message = "Файл успішно завантажено" });
          

        }


        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteFile([FromQuery] string bucketname, [FromQuery] string filename)
        {
              await _service.DeleteFileAsync(bucketname, filename);
           
              return Ok(new ApiResponse<object> { Message = "Файл видалено" });           

        }

        [HttpPut("rename")]
        public async Task<IActionResult> RenameFile([FromQuery] string bucketname, [FromQuery] string filename, [FromQuery] string newfilename)
        {
            if(string.IsNullOrEmpty(filename)) 
                throw new ArgumentNullException("назва файлу не може бути порожньою");

            if (string.IsNullOrWhiteSpace(newfilename)) 
                 throw new ArgumentNullException("назва нового файлу не може бути порожньою");

            if (string.IsNullOrEmpty(bucketname))
                 throw new ArgumentNullException("назва бакету не може бути порожньою");


             await _service.RenameFileAsync(bucketname, filename, newfilename);
            
                return Ok(new ApiResponse<object> { Message = "Файл перейменовано" });
;
        }
    }
}
