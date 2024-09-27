

using FileManager.Api.Dtos;

namespace FileManager.Api.Services.Interfaces;

public interface IExcelService
{
    Task<List<string>> ProcessFileAsync(int batchId, int fileBatchId, FileContentDto fileContent);
}
