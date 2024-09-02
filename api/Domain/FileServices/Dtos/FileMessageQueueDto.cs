
namespace api.Domain.FileServices.Dtos;

public class FileMessageQueueDto
{
    public int BatchId { get; set; }
    public int FileBatchId { get; set; }
    public FileContentDto FileContent { get; set; }
}
