using FileManager.Api.Dtos;

namespace FileManager.Api.Services.Interfaces;

public interface IFileService
{
    Task<FileContentDto> GetFileAsync(string operation, string path);
    Task UploadLargeFilesAsync(Stream stream, string contentType);
    Task DeleteAsync(string operation, string path);
    Task<List<FileDto>> GetAllFileAsync(string operation);
}


