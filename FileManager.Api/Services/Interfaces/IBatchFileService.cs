
using FileManager.Api.Data.Enums;
using FileManager.Api.Dtos;

namespace FileManager.Api.Services.Interfaces;

public interface IBatchService
{
    Task<int> CreateBatchAsync(int qtdyFiles);
    Task<int> AddFileToBatchAsync(int batchId, FileDto fileContent);
}

public interface IBatchFileService
{
    Task FinishFileProcessAsync(int batchFileId, FileStatus status, string message = "");
    Task AddDocumentsToFileAsync(int batchFileId, List<string> documents);
}

public interface IBatchFileDocumentService
{
    Task<List<BatchFileDocumentDto>> GetDocumentsFilesToDeleteAsync(int batchId);
}