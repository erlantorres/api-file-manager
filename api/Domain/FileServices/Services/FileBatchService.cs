
using api.Domain.FileServices.Data.Entities;
using api.Domain.FileServices.Data.Enums;
using api.Domain.FileServices.Data.Repositories.Interfaces;
using api.Domain.FileServices.Dtos;
using api.Domain.FileServices.Services.Interfaces;
using api.Domain.Shared.Helpers;

namespace api.Domain.FileServices.Services;

public class FileBatchService(
    ILogger<IFileBatchService> logger,
    IFileBatchRepository fileBatchRepository
) : IFileBatchService
{

    private static string GetErrorId { get { return Guid.NewGuid().ToString(); } }

    public async Task<int> AddFileToBatchAsync(int batchId, FileDto fileContent)
    {
        try
        {
            return await fileBatchRepository.AddFileToBatchAsync(new FileBatchEntity
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
            return await fileBatchRepository.CreateBatchAsync(qtdyFiles);
        }
        catch (Exception ex)
        {
            var errorId = GetErrorId;
            logger.LogError(ex, $"CreateBatchAsync error id {errorId}: {ex.Message}");
            throw new Exception($"Error to create batch process! Please provide the ID {errorId} to support for assistance.");
        }
    }
}
