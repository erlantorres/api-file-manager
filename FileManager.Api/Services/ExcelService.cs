using FileManager.Api.Dtos;
using FileManager.Api.Services.Interfaces;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace FileManager.Api.Services;

public class ExcelService(IProcessingFileService processingFileService) : IExcelService
{
    public async Task ProcessFileAsync(int fileBatchId, FileContentDto fileContent)
    {
        try
        {
            using var memoryStream = new MemoryStream(fileContent.Content);
            memoryStream.Position = 0;
            var workbook = new XSSFWorkbook(memoryStream);
            await ReadWorkBookAsync(fileContent.Operation, fileBatchId, workbook);
            workbook.Close();
        }
        catch (Exception)
        {
            throw;
        }
    }

    private async Task ReadWorkBookAsync(string operation, int fileBatchId, XSSFWorkbook workbook)
    {
        ISheet sheet;
        for (int sheetIdx = 0; sheetIdx < workbook.NumberOfSheets; sheetIdx++)
        {
            sheet = workbook.GetSheetAt(sheetIdx);
            await ReadSheet(operation, fileBatchId, sheet);
        }
    }

    private async Task ReadSheet(string operation, int fileBatchId, ISheet sheet)
    {
        IRow row;
        for (int rowIdx = 0; rowIdx <= sheet.LastRowNum; rowIdx++)
        {
            row = sheet.GetRow(rowIdx);
            if (rowIdx == 0)
            {
                await processingFileService.ConfigureMappingAsync(operation, row.ToList());
            }
            else
            {
                await processingFileService.AddRowAsync(fileBatchId, row);
            }
        }

        await processingFileService.BulkInsertAsync();
    }
}
