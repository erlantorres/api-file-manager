using FileManager.Api.Data.Entities;

namespace FileManager.Api.Data.Repositories.Interfaces;

public interface IBatchFileDocumentRepository
{
    Task<IEnumerable<FileDocumentEntity>> GetAllAsync();
    Task<IEnumerable<FileDocumentEntity>> GetDocumentEntityAsync(string fileId);
    Task SaveAsync(List<FileDocumentEntity> entities);
}
