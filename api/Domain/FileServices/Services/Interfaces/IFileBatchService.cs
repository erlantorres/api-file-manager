
using api.Domain.FileServices.Dtos;

namespace api.Domain.FileServices.Services.Interfaces;

public interface IFileBatchService
{
    Task<int> AddFilesToBatchAsync(int batchId, FileContentDto fileContent);
    Task<int> CreateBatchAsync();
}
