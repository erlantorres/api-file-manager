
using FileManager.Api.Factories.Enums;
using FileManager.Api.Factories.Interfaces;
using FileManager.Api.Services;
using FileManager.Api.Services.Interfaces;

namespace FileManager.Api.Factories;

public class QueueServiceFactory(IServiceProvider serviceProvider) : IQueueServiceFactory
{
    public IQueueService CreateQueueService(QueueType type) => type switch
    {
        QueueType.RabbitMQ => serviceProvider.GetRequiredService<RabbitMQService>(),
        QueueType.AzureServiceBus => serviceProvider.GetRequiredService<AzureBusService>(),
        _ => throw new ArgumentException("Unknown queue service type")
    };
}
