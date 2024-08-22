
using api.Domain.DatabaseProviders.Services;
using api.Domain.DatabaseServices.Services.Interfaces;
using api.Domain.FileServices.Data.Repositories;
using api.Domain.FileServices.Data.Repositories.Interfaces;
using api.Domain.FileServices.Services;
using api.Domain.FileServices.Services.Interfaces;

namespace api;

public static class Startup
{
    public static IServiceCollection AddDbContextProvider(this IServiceCollection services)
    {
        var connStr = Environment.GetEnvironmentVariable("FILE_DB_CONNSTR") ?? "";
        services.AddSingleton<IDbContext>(new DbContext(connStr));
        return services;
    }

    public static IServiceCollection AddFileServices(this IServiceCollection services)
    {
        services.AddTransient<IFileDatabaseService, FileDatabaseService>();
        services.AddTransient<IFileRepository, FileRepository>();
        return services;
    }
}
