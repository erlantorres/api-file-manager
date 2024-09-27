using FileManager.Api.Dtos;
using FileManager.Api.Services.Interfaces;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace FileManager.Api.Services;

public class ExcelService(
    ILogger<ExcelService> logger,
    IProcessingFileService processingFileService
) : IExcelService
{

    private IList<string> _documents;

    private static string GetErrorId { get { return Guid.NewGuid().ToString(); } }

    public async Task<List<string>> ProcessFileAsync(int batchId, int fileBatchId, FileContentDto fileContent)
    {
        try
        {
            _documents = [];

            using var memoryStream = new MemoryStream(fileContent.Content);
            memoryStream.Position = 0;
            var workbook = new XSSFWorkbook(memoryStream);
            await ReadWorkBookAsync(fileContent.Operation, batchId, fileBatchId, workbook);
            workbook.Close();

            return [.. _documents];
        }
        catch (Exception ex)
        {
            var errorId = GetErrorId;
            logger.LogError(ex, $"ExcelService - ProcessFileAsync:  file batch process id {fileBatchId}, error id {errorId}: {ex.Message}");
            throw new Exception($"Error processing file: fileBatchId {fileBatchId}! Please provide the ID {errorId} to support for assistance.");
        }
    }

    private async Task ReadWorkBookAsync(string operation, int batchId, int fileBatchId, XSSFWorkbook workbook)
    {
        ISheet sheet;
        for (int sheetIdx = 0; sheetIdx < workbook.NumberOfSheets; sheetIdx++)
        {
            sheet = workbook.GetSheetAt(sheetIdx);
            await ReadSheet(operation, batchId, fileBatchId, sheet);
        }
    }

    private async Task ReadSheet(string operation, int batchId, int fileBatchId, ISheet sheet)
    {
        IRow row;
        for (int rowIdx = 0; rowIdx <= sheet.LastRowNum; rowIdx++)
        {
            row = sheet.GetRow(rowIdx);
            if (rowIdx == 0)
            {
                await processingFileService.ConfigureMappingAsync(operation, [.. row]);
            }
            else
            {
                var document = await processingFileService.AddRowAsync(batchId, fileBatchId, row);
                if (!_documents.Contains(document))
                {
                    _documents.Add(document);
                }
            }
        }

        await processingFileService.BulkInsertAsync();
    }
}
