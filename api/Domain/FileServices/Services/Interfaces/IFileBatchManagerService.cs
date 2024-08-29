
using api.Domain.FileServices.Dtos;

namespace api.Domain.FileServices.Services.Interfaces;

public interface IFileBatchManagerService
{
    Task<int> ProcessFilesAsync(List<FileProcessDto> files);
}
