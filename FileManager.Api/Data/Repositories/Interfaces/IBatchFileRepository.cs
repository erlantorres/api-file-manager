
using FileManager.Api.Data.Entities;

namespace FileManager.Api.Data.Repositories.Interfaces;

public interface IBatchFileRepository
{
    Task<int> AddFileToBatchAsync(FileBatchEntity fileBatchEntity);
    Task<IEnumerable<FileBatchEntity>> GetAllAsync();
    Task<FileBatchEntity?> GetByIdAsync(int id);
    Task UpdateAsync(FileBatchEntity batchFile);
}
