
using FileManager.Api.Data.Entities;
using FileManager.Api.Data.Enums;
using FileManager.Api.Data.Repositories.Interfaces;
using FileManager.Api.Dtos;
using FileManager.Api.Helpers;
using FileManager.Api.Services.Interfaces;

namespace FileManager.Api.Services;

public class BatchFileService(
    ILogger<IBatchFileService> logger,
    IBatchRepository batchRepository,
    IBatchFileRepository batchFileRepository,
    IBatchFileDocumentRepository batchFileDocumentRepository
) : IBatchFileService
{

    private static string GetErrorId { get { return Guid.NewGuid().ToString(); } }

    public async Task<int> AddFileToBatchAsync(int batchId, FileDto fileContent)
    {
        try
        {
            return await batchFileRepository.AddFileToBatchAsync(new FileBatchEntity
            {
                BatchId = batchId,
                FileName = fileContent.Name,
                Start = DateTimeHelper.DataHoraDeBrasilia,
                Status = Enum.GetName(typeof(FileStatus), FileStatus.PROCESSING),
            });
        }
        catch (Exception ex)
        {
            var errorId = GetErrorId;
            logger.LogError(ex, $"AddFilesToBatchAsync batch process id {batchId}, error id {errorId}: {ex.Message}");
            throw new Exception($"Error adding file to batch process id {batchId}! Please provide the ID {errorId} to support for assistance.");
        }
    }

    public async Task<int> CreateBatchAsync(int qtdyFiles)
    {
        try
        {
            return await batchRepository.CreateBatchAsync(new BatchEntity
            {
                CreateDate = DateTimeHelper.DataHoraDeBrasilia,
                Status = Enum.GetName(typeof(FileStatus), FileStatus.PROCESSING),
                QtdyFiles = qtdyFiles
            });
        }
        catch (Exception ex)
        {
            var errorId = GetErrorId;
            logger.LogError(ex, $"CreateBatchAsync error id {errorId}: {ex.Message}");
            throw new Exception($"Error to create batch process! Please provide the ID {errorId} to support for assistance.");
        }
    }

    public Task FinishProcessAsync(int batchId)
    {
        throw new NotImplementedException();
    }

    public Task IncrementQtdyProcessedAsync(int batchId, bool processedSuccessfull)
    {
        throw new NotImplementedException();
    }

    public async Task<List<BatchFileDocumentDto>> GetDocumentsFilesToDeleteAsync(int batchId)
    {
        try
        {
            var batches = await batchRepository.GetAllAsync();
            var files = await batchFileRepository.GetAllAsync();
            var documents = await batchFileDocumentRepository.GetAllAsync();

            var query = from document in documents
                        join f in files on document.FileId equals f.Id
                        join document_new in documents on document.DocumentNumber equals document_new.DocumentNumber
                        join files_new in files on document_new.FileId equals files_new.Id
                        join batch_new in batches on files_new.BatchId equals batch_new.Id
                        where
                                files_new.BatchId == 2
                              && batch_new.Status == "PROCESSED"
                              && f.Status == "PROCESSED"
                        group new
                        {
                            f.BatchId,
                            f.Id,
                            document.DocumentNumber
                        } by new
                        {
                            f.BatchId,
                            f.Id,
                            document.DocumentNumber
                        } into g
                        select new BatchFileDocumentDto
                        {
                            BatchId = g.Key.BatchId,
                            FileId = g.Key.Id,
                            DocumentNumber = g.Key.DocumentNumber
                        };

            return query.ToList();
        }
        catch (Exception ex)
        {
            var errorId = GetErrorId;
            logger.LogError(ex, $"GetDocumentsFilesToDeleteAsync error id {errorId}: {ex.Message}");
            throw new Exception($"Error to Get documents to delete! Please provide the ID {errorId} to support for assistance.");
        }
    }
}
