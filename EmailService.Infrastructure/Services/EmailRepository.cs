using EmailService.Domain.Entities;
using EmailService.Domain.Interfaces;
using EmailService.Domain.Queries;
using EmailService.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;


namespace EmailService.Infrastructure.Services
{
    public class EmailRepository : IEmailRepository
    {
        private readonly EmailDbContext _context;

        public EmailRepository(EmailDbContext context)
        {
            _context = context;
        }


        public async Task AddAsync(EmailMessage message)
        {
           await _context.EmailMessages.AddAsync(message);
           await _context.SaveChangesAsync();
        }

        public async Task<EmailMessage?> GetByIdAsync(Guid id)
        {
          var result = await _context.EmailMessages.FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task<PagedResult<EmailMessage>> GetMessagesAsync(EmailQuery query)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(EmailMessage message)
        {
            _context.EmailMessages.Update(message);

            await _context.SaveChangesAsync();
        }
    }
}
