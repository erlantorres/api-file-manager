
using NPOI.SS.UserModel;

namespace FileManager.Api.Services.Interfaces;

public interface IProcessingFileService
{
    Task<string> AddRowAsync(int batchId, int fileBatchId, IRow row);
    Task BulkInsertAsync();
    Task ConfigureMappingAsync(string operation, List<ICell> cells);
}
