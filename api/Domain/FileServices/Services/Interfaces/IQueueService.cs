
using api.Domain.FileServices.Dtos;

namespace api.Domain.FileServices.Services.Interfaces;

public interface IQueueService
{
    Task SendAsync(FileContentDto fileContent);
}
