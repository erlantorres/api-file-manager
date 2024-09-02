
using api.Domain.FileServices.Data.Enums;

namespace api.Domain.FileServices.Services.Interfaces;

public interface IFileDatabaseService : IFileService
{
    Task UpdateFileStatusAsync(string name, FileStatus pROCESSING);
}
