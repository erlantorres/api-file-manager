
using FileManager.Api.Data.Enums;

namespace FileManager.Api.Services.Interfaces;

public interface IFileDatabaseService : IFileService
{
    Task UpdateFileStatusAsync(string operation, string fileName, FileStatus status);
}
