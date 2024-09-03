
using api.Domain.DatabaseServices.Services.Interfaces;
using api.Domain.FileServices.Data.Entities;
using api.Domain.FileServices.Data.Repositories.Interfaces;

namespace api.Domain.FileServices.Data.Repositories;

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
}
