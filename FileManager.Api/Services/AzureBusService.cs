
using FileManager.Api.Dtos;
using FileManager.Api.Services.Interfaces;

namespace FileManager.Api.Services;

public class AzureBusService : IQueueService
{
    public Task SendAsync(FileMessageQueueDto fileMessageQueue)
    {
        throw new NotImplementedException();
    }
}
