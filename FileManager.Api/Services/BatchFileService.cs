
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
) : IBatchService, IBatchFileService, IBatchFileDocumentService
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
                Status = FileStatus.PROCESSING.GetName(),
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
                Status = FileStatus.PROCESSING.GetName(),
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

    private async Task IncrementQtdyProcessedAsync(int batchId, FileStatus status)
    {
        try
        {
            var batch = await batchRepository.GetBatchByIdAsync(batchId);
            if (batch == null || batch.Id <= 0)
            {
                throw new ArgumentNullException(nameof(batch), $"Batch not found for id: {batchId}");
            }

            if (status == FileStatus.PROCESSED)
            {
                batch.QtdyFilesProcessed += 1;
            }
            else
            {
                batch.QtdyFilesFailed += 1;
            }

            var total = batch.QtdyFilesProcessed + batch.QtdyFilesFailed;
            if (batch.QtdyFiles >= total)
            {
                batch.FinishDate = DateTimeHelper.DataHoraDeBrasilia;
                batch.Status = FileStatus.UNDEFINED.GetName();
            }
            else if (batch.QtdyFiles == total)
            {
                batch.FinishDate = DateTimeHelper.DataHoraDeBrasilia;
                batch.Status = FileStatus.PROCESSED.GetName();
            }

            await batchRepository.UpdateAsync(batch);
        }
        catch (Exception ex)
        {
            var errorId = GetErrorId;
            logger.LogError(ex, $"IncrementQtdyProcessedAsync batch process id {batchId}, error id {errorId}: {ex.Message}");
            throw new Exception($"Error increment qtdy file processed to batch process id {batchId}! Please provide the ID {errorId} to support for assistance.");
        }
    }

    public async Task FinishFileProcessAsync(int batchFileId, FileStatus status, string message = "")
    {
        try
        {
            var batchFile = await batchFileRepository.GetByIdAsync(batchFileId);
            if (batchFile == null)
            {
                throw new ArgumentNullException(nameof(batchFile), $"Batch file not found for id: {batchFileId}");
            }

            batchFile.Status = status.GetName();
            batchFile.Finish = DateTimeHelper.DataHoraDeBrasilia;
            batchFile.Message = message;
            await batchFileRepository.UpdateAsync(batchFile);

            await IncrementQtdyProcessedAsync(batchFile.BatchId, status);
        }
        catch (Exception ex)
        {
            var errorId = GetErrorId;
            logger.LogError(ex, $"UpdateFileProcessAsync batch file process id {batchFileId}, error id {errorId}: {ex.Message}");
            throw new Exception($"Error updating the file batch process id {batchFileId}! Please provide the ID {errorId} to support for assistance.");
        }
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

    public async Task AddDocumentsToFileAsync(int batchFileId, List<string> documents)
    {
        try
        {
            var entities = new List<FileDocumentEntity>();
            documents.Distinct().ToList().ForEach(document =>
            {
                entities.Add(new() { FileId = batchFileId, DocumentNumber = document });
            });

            await batchFileDocumentRepository.SaveAsync(entities);
        }
        catch (Exception ex)
        {
            var errorId = GetErrorId;
            logger.LogError(ex, $"AddDocumentsToFileAsync batch file document process id {batchFileId}, error id {errorId}: {ex.Message}");
            throw new Exception($"Error adding the document to file batch process id {batchFileId}! Please provide the ID {errorId} to support for assistance.");
        }
    }
}
