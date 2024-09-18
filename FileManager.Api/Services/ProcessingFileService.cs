
using System.Data;
using FileManager.Api.Data.Entities;
using FileManager.Api.Data.Repositories.Interfaces;
using FileManager.Api.Helpers;
using FileManager.Api.Services.Interfaces;
using NPOI.SS.UserModel;

namespace FileManager.Api.Services;

public class ProcessingFileService(
    IFileMappingRepository fileMappingRepository,
    ITableRepository tableRepository
) : IProcessingFileService
{
    private const int maxRows = 10000;
    private DataTable _dataTable { get; set; }
    private string _operation { get; set; }

    public async Task AddRowAsync(int fileBatchId, IRow row)
    {
        if (_dataTable == null || _dataTable == new DataTable())
        {
            throw new ArgumentNullException(nameof(_dataTable));
        }

        object[] fields = GetFields(fileBatchId, row);
        _dataTable.Rows.Add(fields);

        if (_dataTable.Rows.Count > maxRows)
        {
            await BulkInsertAsync();
        }
    }

    private object[] GetFields(int fileBatchId, IRow row)
    {
        var totalColumn = _dataTable.Columns.Count;
        object[] fields = new object[totalColumn];
        fields[0] = fileBatchId;

        for (int i = 1; i < totalColumn; i++)
        {
            var column = _dataTable.Columns[i];

            var field = row.GetCell(i - 1)?.ToString() ?? "";
            if (string.IsNullOrWhiteSpace(field) || "null".Equals(field.ToString().ToLower()))
            {
                fields[i] = column.AllowDBNull ? DBNull.Value : TableHelper.GetDefaultValue(Type.GetTypeCode(column.DataType));
            }
            else
            {
                field = field.Replace(",", ".");
                fields[i] = TableHelper.GetValue(column.DataType, field);
            }
        }

        return fields;
    }

    public async Task ConfigureMappingAsync(string operation, List<ICell> cells)
    {
        _operation = operation;

        var tableProperties = await GetTableProperties(cells);
        _dataTable = new DataTable(tableProperties.Name);
        _dataTable.Columns.Add("FileBatchId", Type.GetType("System.Int32"));

        string column;
        foreach (var cell in cells)
        {
            column = $"{cell}";
            var tp = tableProperties.Properties.Where(x => x.ColumnName == column).First();

            _dataTable.Columns.Add(column, Type.GetType(tp.Type));
            _dataTable.Columns[column].AllowDBNull = tp.IsNullable;
        }
    }

    private async Task<FileTableMappingEntity> GetTableProperties(List<ICell> cells)
    {
        var fields = string.Join(",", cells);
        var tableMapping = await fileMappingRepository.GetTableNameAsync(_operation, fields);
        if (tableMapping == null || string.IsNullOrWhiteSpace(tableMapping.TableName))
        {
            throw new ArgumentNullException(nameof(tableMapping));
        }

        var properties = await tableRepository.GetTablePropertiesAsync(tableMapping.TableName);
        if (properties == null || properties.Count() >= 0)
        {
            throw new ArgumentNullException(nameof(properties));
        }

        return new FileTableMappingEntity
        {
            Name = tableMapping.TableName,
            Properties = properties.ToList()
        };
    }

    public async Task BulkInsertAsync()
    {
        await tableRepository.BulkInsertAsync(_dataTable);
        _dataTable.Rows.Clear();
    }
}
