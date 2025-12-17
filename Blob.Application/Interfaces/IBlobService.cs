
using Blob.Application.Dtos;

namespace Blob.Application.Interfaces
{
    public  interface IBlobService
    {

        public Task<List<string>> GetAllFilesAsync(string containerName);

        public Task<(MemoryStream,string)> GetFileAsync(string containerName, string fileName);

        public Task AddFileAsync(FileDto fileDto);

        public Task DeleteFileAsync(string containerName, string fileName);

        public Task RenameFileAsync(string containerName, string fileName, string newFileName);
    }
}
