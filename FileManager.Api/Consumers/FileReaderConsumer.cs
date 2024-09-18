
using FileManager.Api.Data.Enums;
using FileManager.Api.Dtos;
using FileManager.Api.Services.Interfaces;
using MassTransit;

namespace FileManager.Api.Consumers;

public class FileReaderConsumer(
    IFileDatabaseService fileDatabaseService,
    IBatchFileService batchFileService,
    IExcelService excelService
) : IConsumer<FileMessageQueueDto>
{
    public async Task Consume(ConsumeContext<FileMessageQueueDto> context)
    {
        try
        {
            var message = context.Message;
            var fileContent = message.FileContent;

            // reade the file
            await excelService.ProcessFileAsync(message.FileBatchId, fileContent);

            // update file to process
            await fileDatabaseService.UpdateFileStatusAsync(fileContent.Operation, fileContent.Name, FileStatus.PROCESSED);

            // increment number of files in batch process
            await batchFileService.IncrementQtdyProcessedAsync(message.BatchId, true);

            //valid if it is the last file to finish the batch
            await batchFileService.FinishProcessAsync(message.BatchId);

            // remove the last data of the same document which is in another batch
            
        }
        catch (Exception ex)
        {

        }
    }
}
