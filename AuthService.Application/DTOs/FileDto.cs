using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.DTOs
{
    public  class FileDto
    {
        public string FileName { get; set; } =string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public Stream Content { get; set; } = null!;
    }
}
