
using System.Data;
using FileManager.Api.Data.Entities;
using FileManager.Api.Data.Providers;
using FileManager.Api.Data.Repositories.Interfaces;

namespace FileManager.Api.Data.Repositories;

public class TableRepository(IDbContext context) : ITableRepository
{
    public async Task BulkInsertAsync(DataTable dataTable)
    {
        await context.BulkCopyAsync(dataTable);
    }

    public async Task<IEnumerable<TablePropertiesEntity>> GetTablePropertiesAsync(string tableName)
    {
        string schema = "dbo";
        string name;

        var nameSplited = tableName.Split('.');
        if (nameSplited.Length > 1)
        {
            schema = nameSplited[0];
            name = nameSplited[1];
        }
        else
        {
            name = nameSplited[0];
        }

        string sql = $@"
            select
                c.name as {nameof(TablePropertiesEntity.ColumnName)},
                case ty.name 
                    when 'varchar' then 'System.String'
                    when 'float' then 'System.Decimal'
                    when 'int' then 'System.Int32'
                    when 'datetime' then 'System.DateTime'
                    when 'date' then 'System.DateTime'
                    when 'long' then 'System.Int64'
                    when 'time' then 'System.TimeOnly'
                    when 'bit' then 'System.Boolean'
                end as {nameof(TablePropertiesEntity.Type)},
                c.is_nullable as {nameof(TablePropertiesEntity.IsNullable)}
            from
                sys.tables t with(nolock)
                inner join sys.schemas s ON t.schema_id = s.schema_id
                inner join sys.columns c on t.object_id = c.object_id
                inner join sys.types ty on c.system_type_id = ty.system_type_id  
            where
                t.name like '%{name}%'
                and s.name = @{nameof(schema)}
        ";

        return await context.QueryAsync<TablePropertiesEntity>(sql, new { schema });
    }
}
