
using api.Domain.DatabaseServices.Services.Interfaces;
using api.Domain.FileServices.Data.Entities;
using api.Domain.FileServices.Data.Repositories.Interfaces;

namespace api.Domain.FileServices.Data.Repositories;

public class FileBatchRepository(IDbContext context) : IFileBatchRepository
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
}
