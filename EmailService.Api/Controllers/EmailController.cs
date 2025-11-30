using EmailService.Application.DTOs;
using EmailService.Application.Interfaces;
using EmailService.Domain.Queries;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace EmailService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }


        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailMessageDto dto)
        {
            var messageId = await _emailService.AddEmailAsync(dto);

            // планируем фоновую задачу через HangFire
            BackgroundJob.Enqueue(() => _emailService.ProcessEmailAsync(messageId));

            return Ok(new ApiResponse { Message = "Повідомлення прийнято для відправлення, перевірте статус", Data = messageId });
        }


        [HttpGet("{id:guid}")]

        public async Task<IActionResult>  GetStatus (Guid id)
        {
            var result = await _emailService.GetByIdAsync(id);

            return Ok(new ApiResponse { Message = "Інформацію успішно отримано", Data = result });
        }


        [HttpGet]
        public async Task<IActionResult> GetMessages([FromQuery] EmailQuery query)
        {
            var result = await _emailService.GetMessagesAsync(query);

            return Ok(new ApiResponse { Message = "Інформацію успішно отримано", Data = result });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteMessage(Guid id)
        {
            await _emailService.DeleteAsync(id);

            return Ok(new ApiResponse { Message = "Повідомлення успішно видалено" });
        }


        [HttpPut("update/{id:guid}")]
        public async Task<IActionResult> UpdateMessage(Guid id, [FromBody] EmailMessageDto dto)
        {
            await _emailService.UpdateAsync(id, dto);

            return Ok(new ApiResponse { Message = "Повідомлення успішно оновлено" });
        }       


    }
}
