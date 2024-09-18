using FileManager.Api.Dtos;
using FileManager.Api.Services.Interfaces;
using MassTransit;

namespace FileManager.Api.Services;

public class RabbitMQService(IBus bus) : IQueueService
{
    public async Task SendAsync(FileMessageQueueDto fileMessageQueue)
    {
        await bus.Send(fileMessageQueue);
    }
}
