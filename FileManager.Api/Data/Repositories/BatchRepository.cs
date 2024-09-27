using FileManager.Api.Data.Entities;
using FileManager.Api.Data.Providers;
using FileManager.Api.Data.Repositories.Interfaces;

namespace FileManager.Api.Data.Repositories;

public class BatchRepository(IDbContext context) : IBatchRepository
{

    private const string _sqlSelect = $@"
    select 
        @{nameof(BatchEntity.CreateDate)}, 
        @{nameof(BatchEntity.FinishDate)}, 
        @{nameof(BatchEntity.Status)}, 
        @{nameof(BatchEntity.QtdyFiles)}, 
        @{nameof(BatchEntity.QtdyFilesProcessed)}
    from
        BatchProcessing with(nolock)
    ";

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

    private const string _sqlUpdate = $@"
    update BatchProcessing 
    set 
        FinishDate = @{nameof(BatchEntity.FinishDate)},
        Status = @{nameof(BatchEntity.Status)},
        QtdyFiles = @{nameof(BatchEntity.QtdyFiles)},
        QtdyFilesProcessed = @{nameof(BatchEntity.QtdyFilesProcessed)}
    where 
        id = @{nameof(BatchEntity.Id)}
    ";

    public async Task<int> CreateBatchAsync(BatchEntity batchEntity)
    {
        string sql = $@"{_sqlInsert} select scope_identity() as id";
        return await context.QueryFirstAsync<int>(sql, batchEntity);
    }

    public async Task<IEnumerable<BatchEntity>> GetAllAsync()
    {
        return await context.QueryAsync<BatchEntity>(_sqlSelect);
    }

    public async Task<BatchEntity?> GetBatchByIdAsync(int batchId)
    {
        string sql = $"{_sqlSelect} where id = @{nameof(batchId)}";
        return await context.QueryFirstOrDefaultAsync<BatchEntity>(sql, new { batchId });
    }

    public async Task UpdateAsync(BatchEntity batch)
    {
        await context.ExecuteAsync(_sqlUpdate, batch);
    }
}
