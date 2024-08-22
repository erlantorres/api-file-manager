
using api.Domain.FileServices.Dtos;

namespace api.Domain.FileServices.Services.Interfaces;

public interface IFileService
{
    Task<FileDto> Download(string path);
    Task Upload(FileDto file);
    Task UploadLargeFiles(Stream stream, string contentType);
    Task Delete(string path);
}


