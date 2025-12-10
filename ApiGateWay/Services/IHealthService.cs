using ApiGateWay.Models;

namespace ApiGateWay.Services
{
    public interface IHealthService
    {

         Task<ApiResponse<object>> GetAllCheckHealth();
    }
}
