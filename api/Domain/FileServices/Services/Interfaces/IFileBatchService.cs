
using api.Domain.FileServices.Dtos;

namespace api.Domain.FileServices.Services.Interfaces;

public interface IFileBatchService
{
    Task<int> AddFileToBatchAsync(int batchId, FileDto fileContent);
    Task<int> CreateBatchAsync(int qtdyFiles);
}
