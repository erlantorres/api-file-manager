
using api.Domain.FileServices.Dtos;

namespace api.Domain.FileServices.Services.Interfaces;

public interface IFileService
{
    Task<FileDto> GetFileAsync(string operation, string path);
    Task UploadLargeFilesAsync(Stream stream, string contentType);
    Task DeleteAsync(string operation, string path);
    Task<List<FileDto>> GetAllFileAsync(string operation);
    Task<FileContentDto> DownloadAsync(string operation, string path);
}


