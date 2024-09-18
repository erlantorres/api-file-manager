
using FileManager.Api.Dtos;

namespace FileManager.Api.Services.Interfaces;

public interface IQueueService
{
    Task SendAsync(FileMessageQueueDto fileMessageQueue);
}
