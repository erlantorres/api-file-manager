using System.Reflection;
using FileManager.Api.Data.Providers;
using FileManager.Api.Data.Repositories;
using FileManager.Api.Data.Repositories.Interfaces;
using FileManager.Api.Factories;
using FileManager.Api.Factories.Interfaces;
using FileManager.Api.Services;
using FileManager.Api.Services.Interfaces;
using MassTransit;

namespace FileManager.Api;

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
        services.AddServices();
        services.AddFactories();
        services.AddRepositories();
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IBatchFileService, BatchFileService>();
        services.AddTransient<IExcelService, ExcelService>();
        services.AddTransient<IFileDatabaseService, FileDatabaseService>();
        services.AddTransient<IProcessingFileManagerService, ProcessingFileManagerService>();
        services.AddTransient<IProcessingFileService, ProcessingFileService>();

        services.AddTransient<RabbitMQService>();
        services.AddTransient<AzureBusService>();

        return services;
    }

    private static IServiceCollection AddFactories(this IServiceCollection services)
    {
        services.AddTransient<IQueueServiceFactory, QueueServiceFactory>();
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IFileRepository, FileRepository>();
        services.AddTransient<IFileMappingRepository, FileMappingRepository>();
        services.AddTransient<ITableRepository, TableRepository>();
        services.AddTransient<IBatchRepository, BatchRepository>();
        services.AddTransient<IBatchFileRepository, BatchFileRepository>();
        services.AddTransient<IBatchFileDocumentRepository, BatchFileDocumentRepository>();
        return services;
    }

    public static IServiceCollection AddQueueService(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            x.SetInMemorySagaRepositoryProvider();

            var entryAssembly = Assembly.GetEntryAssembly();
            x.AddConsumers(entryAssembly);
            x.AddSagaStateMachines(entryAssembly);
            x.AddSagas(entryAssembly);
            x.AddActivities(entryAssembly);

            x.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
