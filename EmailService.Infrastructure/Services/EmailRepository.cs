using EmailService.Application.Exceptions;
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

        public async Task<EmailMessage> GetByIdAsync(Guid id)
        {
          var result = await _context.EmailMessages.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
                throw new NotFoundException("повідомлення не знайдено");
            return result;
        }

        public async Task<PagedResult<EmailMessage>> GetMessagesAsync(EmailQuery query)
        {
            var emails = _context.EmailMessages.AsQueryable();

            if(query.UserId != Guid.Empty  || query.UserId != null)
            {
                emails = emails.Where(x => x.UserId == query.UserId);
            }

            if(query.IsSent != null)
            {
                emails = emails.Where(x => x.IsSent == query.IsSent);
            }

            if(string.IsNullOrEmpty(query.To))
            {
                emails = emails.Where(x=> x.To == query.To);
            }           

            var total = await emails.CountAsync();

            var result = await emails
                   .OrderByDescending(x => x.CreatedAt)
                   .Skip((query.Page - 1) * query.PageSize)
                   .Take(query.PageSize)
                   .ToListAsync();

            return new PagedResult<EmailMessage>
            {
                Items = result,
                TotalCount = total,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task UpdateAsync(EmailMessage message)
        {
            var curentMessage = await GetByIdAsync(message.Id);
            if (curentMessage == null)
                throw new NotFoundException("повідомлення не знайдено");

            _context.EmailMessages.Update(message);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var message = await _context.EmailMessages.FirstOrDefaultAsync(x=> x.Id == id);
            if (message != null)
                throw new NotFoundException("повідомлення не знайдено");

            _context.EmailMessages.Remove(message);
            await _context.SaveChangesAsync();
        }
    }
}
