
using api.Domain.FileServices.Data.Entities;
using api.Domain.FileServices.Dtos;

namespace api.Domain.FileServices.Data.Repositories.Interfaces;

public interface IFileRepository
{
    Task SaveAsync(FileEntity fileEntity);
    Task<FileEntity> GetAsync(string fileName);
    Task<IEnumerable<FileEntity>> GetAllAsync(string operation);
    Task DeleteAsync(FileDeleteDto parameters);
}
