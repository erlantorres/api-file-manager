
namespace FileManager.Api.Dtos;

public class BatchDto
{
    public int Id { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? FinishDate { get; set; }
    public int QtdyFiles { get; set; }
    public int QtdyFilesProcessed { get; set; }
}
