
using api.Domain.FileServices.Data.Entities;

namespace api.Domain.FileServices.Data.Repositories.Interfaces;

public interface IFileRepository
{
    Task Save(FileEntity fileEntity);
    Task<FileEntity> Get(string name);
    Task<IEnumerable<FileEntity>> GetAll(string operation);
    Task Delete(string name);
}
