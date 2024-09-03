
using api.Domain.FileServices.Data.Entities;

namespace api.Domain.FileServices.Data.Repositories.Interfaces;

public interface IFileBatchRepository
{
    Task<int> AddFileToBatchAsync(FileBatchEntity fileBatchEntity);
    Task<int> CreateBatchAsync(int qtdyFiles);
}
