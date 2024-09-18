using FileManager.Api.Factories.Enums;
using FileManager.Api.Services.Interfaces;

namespace FileManager.Api.Factories.Interfaces;

public interface IQueueServiceFactory
{
    IQueueService CreateQueueService(QueueType type);
}