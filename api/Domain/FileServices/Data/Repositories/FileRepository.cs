
using api.Domain.DatabaseServices.Services.Interfaces;
using api.Domain.FileServices.Data.Entities;
using api.Domain.FileServices.Data.Repositories.Interfaces;

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


    public Task Delete(string name)
    {
        throw new NotImplementedException();
    }

    public Task<FileEntity> Get(string name)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<FileEntity>> GetAll(string operation)
    {
        throw new NotImplementedException();
    }

    public async Task Save(FileEntity fileEntity)
    {
        await context.ExecuteAsync(_sqlInsert, fileEntity);
    }
}
