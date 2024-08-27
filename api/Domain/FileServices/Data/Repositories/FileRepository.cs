
using api.Domain.DatabaseServices.Services.Interfaces;
using api.Domain.FileServices.Data.Entities;
using api.Domain.FileServices.Data.Repositories.Interfaces;
using api.Domain.FileServices.Dtos;

namespace api.Domain.FileServices.Data.Repositories;

public class FileRepository(IDbContext context) : IFileRepository
{

    private const string _sqlQuery = $@"
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

    public async Task DeleteAsync(FileDeleteDto parameters)
    {
        var delete = @$"delete FileUpload where Name = @{nameof(parameters.FileName)} and Operation = @{nameof(parameters.Operation)}";
        await context.ExecuteAsync(delete, parameters);
    }

    public Task<FileEntity> GetAsync(string fileName)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<FileEntity>> GetAllAsync(string operation)
    {
        throw new NotImplementedException();
    }

    public async Task SaveAsync(FileEntity fileEntity)
    {
        await context.ExecuteAsync(_sqlInsert, fileEntity);
    }
}
