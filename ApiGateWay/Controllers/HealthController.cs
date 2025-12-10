using ApiGateWay.Models;
using ApiGateWay.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateWay.Controllers
{
    [Route("health")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly IHealthService _service;

        public HealthController(IHealthService service)
        {
           _service = service;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllHealth()
        {
           
           var result = await  _service.GetAllCheckHealth();
            return Ok(result);
            

         }

      


        
    }
}
