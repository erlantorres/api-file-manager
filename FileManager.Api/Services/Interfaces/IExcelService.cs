

using FileManager.Api.Dtos;

namespace FileManager.Api.Services.Interfaces;

public interface IExcelService
{
    Task ProcessFileAsync(int fileBatchId, FileContentDto fileContent);
}
