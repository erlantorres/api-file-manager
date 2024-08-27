
using api.Domain.FileServices.Dtos;

namespace api.Domain.FileServices.Services.Interfaces;

public interface IFileService
{
    Task<FileDto> DownloadAsync(string fileName, string operation);
    Task UploadLargeFilesAsync(Stream stream, string contentType);
    Task DeleteAsync(string operation, string fileName);
}


