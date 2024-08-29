
using api.Domain.FileServices.Dtos;
using api.Domain.FileServices.Services.Interfaces;

namespace api.Domain.FileServices.Services;

public class FileBatchManagerService(
    IFileService fileService,
    IFileBatchService fileBatchManagerService
) : IFileBatchManagerService
{
    public async Task<int> ProcessFilesAsync(List<FileProcessDto> files)
    {
        // create lote 
        var batchId = await fileBatchManagerService.CreateBatchAsync();

        files.ForEach(async file =>
        {
            // get files from base
            var fileContent = await fileService.GetFileAsync(file.Operation, file.Name);
            if (fileContent == null || fileContent.Content.Length == 0)
            {
                continue;
            }

            await fileBatchManagerService.AddFilesToBatchAsync(batchId, fileContent);
        });



        throw new NotImplementedException();
    }
}
