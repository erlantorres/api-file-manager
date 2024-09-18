
using FileManager.Api.Data.Entities;

namespace FileManager.Api.Data.Repositories.Interfaces;

public interface IFileMappingRepository
{
    Task<FileMapingEntity?> GetTableNameAsync(string operation, string fields);
}
