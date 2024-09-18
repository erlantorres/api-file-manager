using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;


namespace FileManager.Api.Data.Providers;

public class DbContext : IDbContext
{
    private readonly string _connectionString;
    private readonly int? _timeout;

    public DbContext(string connectionString, int? timeout = null)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString), "Connection string connot be null!");
        }

        _connectionString = connectionString;
        _timeout = timeout;
    }

    private SqlConnection GetOpenConnection()
    {
        var conn = new SqlConnection(_connectionString); ;
        conn.Open();
        return conn;
    }

    public virtual async Task<int> ExecuteAsync(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null)
    {
        using var conn = GetOpenConnection();
        return await conn.ExecuteAsync(sql, param, transaction, commandTimeout ?? _timeout, CommandType.Text);
    }

    public virtual async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null)
    {
        using var conn = GetOpenConnection();
        return await conn.QueryAsync<T>(sql, param, transaction, commandTimeout ?? _timeout, CommandType.Text);
    }

    public virtual async Task<T> QueryFirstAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null)
    {
        using var conn = GetOpenConnection();
        return await conn.QueryFirstAsync<T>(sql, param, transaction, commandTimeout ?? _timeout, CommandType.Text);
    }

    public virtual async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null)
    {
        using var conn = GetOpenConnection();
        return await conn.QueryFirstOrDefaultAsync<T>(sql, param, transaction, commandTimeout ?? _timeout, CommandType.Text);
    }

    public async Task BulkCopyAsync(DataTable dataTable)
    {
        using var sqlBulkCopy = new SqlBulkCopy(_connectionString)
        {
            DestinationTableName = dataTable.TableName
        };

        int length = dataTable.Columns.Count;
        for (int j = 0; j < length; j++)
        {
            sqlBulkCopy.ColumnMappings.Add(dataTable.Columns[j].ColumnName, dataTable.Columns[j].ColumnName);
        }

        await sqlBulkCopy.WriteToServerAsync(dataTable);
    }
}
