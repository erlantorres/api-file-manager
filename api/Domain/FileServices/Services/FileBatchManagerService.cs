
using api.Domain.FileServices.Data.Enums;
using api.Domain.FileServices.Dtos;
using api.Domain.FileServices.Services.Interfaces;

namespace api.Domain.FileServices.Services;

public class FileBatchManagerService(
    IFileDatabaseService fileDatabaseService,
    IFileBatchService fileBatchService,
    IQueueService queueService
) : IFileBatchManagerService
{
    public async Task<int> ProcessFilesAsync(List<FileProcessDto> files)
    {
        // create lote 
        var batchId = await fileBatchService.CreateBatchAsync(files.Count);

        files.ForEach(async file =>
        {
            // get files with content from base
            var fileContent = await fileDatabaseService.GetFileAsync(file.Operation, file.Name);
            if (fileContent != null && fileContent.Content != null && fileContent.Content.Length > 0)
            {
                // add file in the batch
                var fileBatchId = await fileBatchService.AddFileToBatchAsync(batchId, fileContent);

                // update file to processing
                await fileDatabaseService.UpdateFileStatusAsync(fileContent.Operation, fileContent.Name, FileStatus.PROCESSING);

                // send file to queue
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
