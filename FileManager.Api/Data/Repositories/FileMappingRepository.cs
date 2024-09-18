using FileManager.Api.Data.Entities;
using FileManager.Api.Data.Providers;
using FileManager.Api.Data.Repositories.Interfaces;

namespace FileManager.Api.Data.Repositories;

public class FileMappingRepository(IDbContext context) : IFileMappingRepository
{

    private const string _sqlSelect = $@"
     select 
         Id as {nameof(FileMapingEntity.Id)},
         Operation as {nameof(FileMapingEntity.Operation)},
         Header as {nameof(FileMapingEntity.Header)},
         TableName as {nameof(FileMapingEntity.TableName)}
     from 
         FileTableMapping with(nolock)
 ";

    public async Task<FileMapingEntity?> GetTableNameAsync(string operation, string fields)
    {
        var sql = $@"{_sqlSelect} where operation = @{nameof(operation)} and header = @{nameof(fields)}";
        return await context.QueryFirstOrDefaultAsync<FileMapingEntity>(sql, new { operation, fields });
    }
}
