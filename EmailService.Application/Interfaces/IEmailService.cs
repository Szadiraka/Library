using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService.Application.Interfaces
{
    public interface IEmailService
    {
        Task<Guid> QueueEmailAsync(string to, string subject, string body);

        Task ProcessEmailAsync(Guid messageId);
    }
}
