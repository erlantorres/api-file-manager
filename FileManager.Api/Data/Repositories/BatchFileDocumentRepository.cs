using FileManager.Api.Data.Entities;
using FileManager.Api.Data.Providers;
using FileManager.Api.Data.Repositories.Interfaces;

namespace FileManager.Api.Data.Repositories;

public class BatchFileDocumentRepository(IDbContext context) : IBatchFileDocumentRepository
{
    private const string _sqlSelect = $@"
    select 
        FileId as {nameof(FileDocumentEntity.FileId)},
        DocumentNumber as {nameof(FileDocumentEntity.DocumentNumber)}
    from 
        BatchFileDocument with(nolock)
    ";

    private const string _sqlInsert = $@"
    insert into BatchFileDocument (FileId, DocumentNumber)
    values (
        @{nameof(FileDocumentEntity.FileId)},
        @{nameof(FileDocumentEntity.DocumentNumber)}
    )
    ";

    public async Task<IEnumerable<FileDocumentEntity>> GetAllAsync()
    {
        return await context.QueryAsync<FileDocumentEntity>(_sqlSelect);
    }

    public async Task<IEnumerable<FileDocumentEntity>> GetDocumentEntityAsync(string fileId)
    {
        var sql = $@"{_sqlSelect} where fileId = @{nameof(fileId)}";
        return await context.QueryAsync<FileDocumentEntity>(sql, new { fileId });
    }

    public async Task SaveAsync(List<FileDocumentEntity> entities)
    {
        await context.ExecuteAsync(_sqlInsert, entities);
    }
}
