using System.Data;
using FileManager.Api.Data.Entities;

namespace FileManager.Api.Data.Repositories.Interfaces;

public interface ISystemTableRepository
{
    Task BulkInsertAsync(DataTable dataTable);
    Task<IEnumerable<TablePropertiesEntity>> GetTablePropertiesAsync(string tableName);
}
