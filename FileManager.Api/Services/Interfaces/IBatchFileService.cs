
using FileManager.Api.Dtos;

namespace FileManager.Api.Services.Interfaces;

public interface IBatchFileService
{
    Task<int> AddFileToBatchAsync(int batchId, FileDto fileContent);
    Task<int> CreateBatchAsync(int qtdyFiles);
    Task FinishProcessAsync(int batchId);
    Task IncrementQtdyProcessedAsync(int batchId, bool processedSuccessfull);
    Task<List<BatchFileDocumentDto>> GetDocumentsFilesToDeleteAsync(int batchId);
}
