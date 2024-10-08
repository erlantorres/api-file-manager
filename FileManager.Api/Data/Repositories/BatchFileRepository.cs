using FileManager.Api.Data.Entities;
using FileManager.Api.Data.Providers;
using FileManager.Api.Data.Repositories.Interfaces;

namespace FileManager.Api.Data.Repositories;

public class BatchFileRepository(IDbContext context) : IBatchFileRepository
{

    private const string _sqlInsert = $@"
    insert into BatchFile (BatchId, Start, Finish, Status, FileName, Message)
    values (
        @{nameof(FileBatchEntity.BatchId)},
        @{nameof(FileBatchEntity.Start)},
        @{nameof(FileBatchEntity.Finish)},
        @{nameof(FileBatchEntity.Status)},
        @{nameof(FileBatchEntity.FileName)},
        @{nameof(FileBatchEntity.Message)}
    )
    ";

    public async Task<int> AddFileToBatchAsync(FileBatchEntity fileBatchEntity)
    {
        string sql = $"{_sqlInsert} select scope_identity() as id";
        return await context.QueryFirstAsync<int>(sql, fileBatchEntity);
    }

    public Task<IEnumerable<FileBatchEntity>> GetAllAsync()
    {
        throw new NotImplementedException();
    }
}
