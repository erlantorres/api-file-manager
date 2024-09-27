
using FileManager.Api.Data.Enums;
using FileManager.Api.Dtos;
using FileManager.Api.Services.Interfaces;
using MassTransit;

namespace FileManager.Api.Consumers;

public class FileReaderConsumer(
    ILogger<FileReaderConsumer> logger,
    IFileDatabaseService fileDatabaseService,
    IBatchFileService batchFileService,
    IExcelService excelService
) : IConsumer<FileMessageQueueDto>
{
    public async Task Consume(ConsumeContext<FileMessageQueueDto> context)
    {
        var message = context.Message;
        var fileContent = message.FileContent;

        try
        {
            // reade the file
            var documents = await excelService.ProcessFileAsync(message.BatchId, message.FileBatchId, fileContent);

            // update file to process
            await fileDatabaseService.UpdateFileStatusAsync(fileContent.Operation, fileContent.Name, FileStatus.PROCESSED);

            // add documentos related to the files
            await batchFileService.AddDocumentsToFileAsync(message.FileBatchId, documents);

            // update status file
            // increment number of files in batch process
            //valid if it is the last file to finish the batch
            await batchFileService.FinishFileProcessAsync(message.FileBatchId, FileStatus.PROCESSED);

            // remove the last data of the same document which is in another batch
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"FileReaderConsumer - Error: {ex.Message}");
            await batchFileService.FinishFileProcessAsync(message.FileBatchId, FileStatus.PROCESSING_FAILED, ex.Message);
        }
    }
}
