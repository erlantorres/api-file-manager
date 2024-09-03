
using api.Domain.FileServices.Data.Entities;

namespace api.Domain.FileServices.Data.Repositories.Interfaces;

public interface IFileRepository
{
    Task SaveAsync(FileEntity fileEntity);
    Task<FileEntity?> GetWithContentAsync(string operation, string fileName);
    Task<FileEntity?> GetAsync(string operation, string fileName);
    Task<IEnumerable<FileEntity>> GetAllAsync(string operation);
    Task DeleteAsync(string operation, string fileName);
    Task UpdateFileStatusAsync(string operation, string fileName, string status);
}
