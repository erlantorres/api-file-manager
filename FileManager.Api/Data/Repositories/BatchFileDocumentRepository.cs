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

    public Task<IEnumerable<FileDocumentEntity>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<FileDocumentEntity>> GetDocumentEntityAsync(string fileId)
    {
        var sql = $@"{_sqlSelect} where fileId = @{nameof(fileId)}";
        return await context.QueryAsync<FileDocumentEntity>(sql, new { fileId });
    }
}
