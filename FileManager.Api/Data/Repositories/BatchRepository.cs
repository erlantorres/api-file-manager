using FileManager.Api.Data.Entities;
using FileManager.Api.Data.Providers;
using FileManager.Api.Data.Repositories.Interfaces;

namespace FileManager.Api.Data.Repositories;

public class BatchRepository(IDbContext context) : IBatchRepository
{
    private const string _sqlInsert = $@"
    insert into BatchProcessing (CreateDate, FinishDate, Status, QtdyFiles, QtdyFilesProcessed)
    values (
        @{nameof(BatchEntity.CreateDate)}, 
        @{nameof(BatchEntity.FinishDate)}, 
        @{nameof(BatchEntity.Status)}, 
        @{nameof(BatchEntity.QtdyFiles)}, 
        @{nameof(BatchEntity.QtdyFilesProcessed)}
    )
    ";

    public async Task<int> CreateBatchAsync(BatchEntity batchEntity)
    {
        string sql = $@"{_sqlInsert} select scope_identity() as id";
        return await context.QueryFirstAsync<int>(sql, batchEntity);
    }

    public Task<IEnumerable<BatchEntity>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<BatchEntity> GetBatchByIdAsync(int batchId)
    {
        throw new NotImplementedException();
    }
}
