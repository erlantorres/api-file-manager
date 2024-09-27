
using FileManager.Api.Data.Entities;

namespace FileManager.Api.Data.Repositories.Interfaces;

public interface IBatchRepository
{
    Task<int> CreateBatchAsync(BatchEntity batchEntity);
    Task<IEnumerable<BatchEntity>> GetAllAsync();
    Task<BatchEntity?> GetBatchByIdAsync(int batchId);
    Task UpdateAsync(BatchEntity batch);
}
