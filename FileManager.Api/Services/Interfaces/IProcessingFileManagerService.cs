
using FileManager.Api.Dtos;

namespace FileManager.Api.Services.Interfaces;

public interface IProcessingFileManagerService
{
    Task<int> ProcessFilesAsync(List<FileProcessDto> files);
}
