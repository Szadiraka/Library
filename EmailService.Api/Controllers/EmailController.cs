using EmailService.Application.DTOs;
using EmailService.Application.Exceptions;
using EmailService.Application.Interfaces;
using EmailService.Domain.Queries;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "AuthService")]
        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
        {
            if (!ModelState.IsValid)
                throw new _ValidationException("Дані повідомлення некоректні");

            if (request.Data.Count == 0) 
                throw new _ValidationException("Словник повідомлення повинен бути заповненим");

            var messageId = await _emailService.AddEmailAsync(request.Dto);

       
            BackgroundJob.Enqueue(() => _emailService.ProcessEmailAsync(messageId, request.Template!.ToString(), request.Data));

            return Ok(new ApiResponse { Message = "Повідомлення прийнято для відправлення, перевірте статус", Data = messageId });
        }





        [Authorize(Roles = "Admin")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult>  GetStatus (Guid id)
        {
            var result = await _emailService.GetByIdAsync(id);

            return Ok(new ApiResponse { Message = "Інформацію успішно отримано", Data = result });
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetMessages([FromQuery] EmailQuery query)
        {
            var result = await _emailService.GetMessagesAsync(query);

            return Ok(new ApiResponse { Message = "Інформацію успішно отримано", Data = result });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id:guid}")]
        public async Task<IActionResult> DeleteMessage(Guid id)
        {
            await _emailService.DeleteAsync(id);

            return Ok(new ApiResponse { Message = "Повідомлення успішно видалено" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("update/{id:guid}")]
        public async Task<IActionResult> UpdateMessage(Guid id, [FromBody] EmailMessageDto dto)
        {
            await _emailService.UpdateAsync(id, dto);

            return Ok(new ApiResponse { Message = "Повідомлення успішно оновлено" });
        }       


    }
}
