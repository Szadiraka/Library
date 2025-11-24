using AuthService.Application.DTOs;


namespace AuthService.Application.Interfaces
{
    public  interface IBlobStorageService
    {
        Task<string> UploadFileAsync(FileDto file, string userId);

    }
}
