
using FileManager.Api.Data.Enums;
using FileManager.Api.Dtos;
using FileManager.Api.Factories.Enums;
using FileManager.Api.Factories.Interfaces;
using FileManager.Api.Services.Interfaces;

namespace FileManager.Api.Services;

public class ProcessingFileManagerService(
    IFileDatabaseService fileDatabaseService,
    IBatchService batchService,
    IQueueServiceFactory queueServiceFactory
) : IProcessingFileManagerService
{
    public async Task<int> ProcessFilesAsync(List<FileProcessDto> files)
    {
        // create lote 
        var batchId = await batchService.CreateBatchAsync(files.Count);

        files.ForEach(async file =>
        {
            // get files with content from base
            var fileContent = await fileDatabaseService.GetFileAsync(file.Operation, file.Name);
            if (fileContent != null && fileContent.Content != null && fileContent.Content.Length > 0)
            {
                // add file in the batch
                var fileBatchId = await batchService.AddFileToBatchAsync(batchId, fileContent);

                // update file to processing
                await fileDatabaseService.UpdateFileStatusAsync(fileContent.Operation, fileContent.Name, FileStatus.PROCESSING);

                // send file to queue
                var queueService = queueServiceFactory.CreateQueueService(QueueType.RabbitMQ);
                await queueService.SendAsync(new FileMessageQueueDto
                {
                    BatchId = batchId,
                    FileBatchId = fileBatchId,
                    FileContent = fileContent
                });
            }
        });

        return batchId;
    }
}
