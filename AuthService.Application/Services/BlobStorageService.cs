using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Domain.Exceptions;



namespace AuthService.Application.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        public Task<string> UploadFileAsync(FileDto file, string userId)
        {
            if (file == null || file.Content == null)
                throw new NotFoundException("файл не знайдено");

            return Task.FromResult($"http://localhost:5001/avatar/{file.FileName}");
        }
    }
    
}
