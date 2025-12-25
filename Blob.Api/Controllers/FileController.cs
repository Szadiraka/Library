using Blob.Api.Mappers;
using Blob.Api.Requests;
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
    public class FileController : ControllerBase
    {
        private readonly IFileMetaDataService _service;
        private readonly IUserContextService _userService;

        public FileController(IFileMetaDataService service, IUserContextService userService)
        {
            _service = service;
            _userService = userService;
        }

        [HttpGet("download/{id:guid}")]
        public  async Task<IActionResult> GetFileById(Guid id)
        {         

            (MemoryStream? stream, string? contentType, string? fileName ) = await  _service.GetFileByIdAsync(id);
            if (stream == null || contentType == null || fileName == null)
              return NotFound(new ApiResponse<object> { Message = "Файл не знайдено, або відсутні дані" });
           
            
            stream.Position = 0;
            return File(stream, contentType, fileName);
            

              
        }

        [HttpGet("getinfo/{id:guid}")]
        public async Task<IActionResult> GetFileMetaDataById(Guid id)
        {
           FileMetaData result = await _service.FileMetaDataByIdAsync(id);

            return Ok(new ApiResponse<FileMetaData> { Message = "Iнформація про файл отримано", Data = result });
        }

       
        [HttpGet("list")]
        public async Task<IActionResult> GetAllFiles([FromQuery] FileQuery query)
        {  
            if(!ModelState.IsValid)
                throw new _ValidationException("Невалідні дані");

            PagedResult<FileMetaData> result = await _service.GetAllFileMetadatasAsync(query);       

            return Ok(new ApiResponse<PagedResult<FileMetaData>> {Message = "Перелік файлів отримано", Data = result });
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile( [FromForm] FileRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object> { Message = "Не валідні дані" });
            }
            string? userId = _userService.UserId;

            FileMetaData file = await FileMapper.ToDto(request);

            if(userId != null)
                file.UserId = Guid.Parse(userId);           

            await _service.AddFileAsync(file);
           
            return Ok(new ApiResponse<object> { Message = "Файл успішно додано" });
          

        }


        [HttpDelete("delete/{id:guid}")]
        public async Task<IActionResult> DeleteFile(Guid id)
        {
              await _service.DeleteFileAsync(id);
           
              return Ok(new ApiResponse<object> { Message = "Файл видалено" });           

        }

        [HttpPut("rename")]
        public async Task<IActionResult> RenameFile([FromQuery] Guid id,[FromQuery] string newname, bool exceptionIfExist = false)
        {
            if(Guid.Empty == id)
                throw new ArgumentNullException("id не може бути порожнім");

            if (string.IsNullOrWhiteSpace(newname)) 
                 throw new ArgumentNullException("назва нового файлу не може бути порожньою");           


             await _service.RenameFileAsync(id, newname, exceptionIfExist);
            
                return Ok(new ApiResponse<object> { Message = "Файл перейменовано" });
;
        }


    }
}
