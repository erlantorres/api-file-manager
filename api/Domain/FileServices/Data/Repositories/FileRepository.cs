
using api.Domain.DatabaseServices.Services.Interfaces;
using api.Domain.FileServices.Data.Entities;
using api.Domain.FileServices.Data.Repositories.Interfaces;

namespace api.Domain.FileServices.Data.Repositories;

public class FileRepository(IDbContext context) : IFileRepository
{
    private const string _sqlQueryWithoutContent = $@"
        select
            {nameof(FileEntity.Name)},
            {nameof(FileEntity.Operation)},
            {nameof(FileEntity.CreateDate)},
            {nameof(FileEntity.Status)},
            {nameof(FileEntity.UnTrustedName)},
            {nameof(FileEntity.Size)}
        from
            FileUpload with(nolock)
    ";

    private const string _sqlQueryWithContent = $@"
        select
            {nameof(FileEntity.Name)},
            {nameof(FileEntity.Operation)},
            {nameof(FileEntity.CreateDate)},
            {nameof(FileEntity.Status)},
            {nameof(FileEntity.UnTrustedName)},
            {nameof(FileEntity.Size)},
            {nameof(FileEntity.Content)}
        from
            FileUpload with(nolock)
    ";

    private const string _sqlInsert = $@"
        insert into FileUpload (Name, Operation, CreateDate, Status, UnTrustedName, Size, Content)
        values(
            @{nameof(FileEntity.Name)}, 
            @{nameof(FileEntity.Operation)}, 
            @{nameof(FileEntity.CreateDate)}, 
            @{nameof(FileEntity.Status)}, 
            @{nameof(FileEntity.UnTrustedName)}, 
            @{nameof(FileEntity.Size)}, 
            @{nameof(FileEntity.Content)}
        )
    ";

    public async Task DeleteAsync(string operation, string fileName)
    {
        var delete = @$"delete FileUpload where Name = @{nameof(fileName)} and Operation = @{nameof(operation)}";
        await context.ExecuteAsync(delete, new { fileName, operation });
    }

    public async Task<FileEntity?> GetWithContentAsync(string operation, string fileName)
    {
        var sql = @$"{_sqlQueryWithContent} where Name = @{nameof(fileName)} and Operation = @{nameof(operation)}";
        return await context.QueryFirstOrDefaultAsync<FileEntity>(sql, new { fileName, operation });
    }

    public async Task<FileEntity?> GetAsync(string operation, string fileName)
    {
        var sql = @$"{_sqlQueryWithoutContent} where Name = @{nameof(fileName)} and Operation = @{nameof(operation)}";
        return await context.QueryFirstOrDefaultAsync<FileEntity>(sql, new { fileName, operation });
    }

    public async Task<IEnumerable<FileEntity>> GetAllAsync(string operation)
    {
        var sql = @$"{_sqlQueryWithoutContent} where  Operation = @{nameof(operation)}";
        return await context.QueryAsync<FileEntity>(sql, new { operation });
    }

    public async Task SaveAsync(FileEntity fileEntity)
    {
        await context.ExecuteAsync(_sqlInsert, fileEntity);
    }

    public async Task UpdateFileStatusAsync(string operation, string fileName, string status)
    {
        string update = $@"update FileUpload set status = @{nameof(status)} where Name = @{nameof(fileName)} and Operation = @{nameof(operation)}";
        await context.ExecuteAsync(update, new { operation, fileName, status });
    }
}
