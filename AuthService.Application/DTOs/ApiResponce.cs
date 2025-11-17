using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.DTOs
{
    public  class ApiResponse
    {
        public string? Message { get; set; }
        public object? Data { get; set; }
       
    }
}
