
namespace FileManager.Api.Data.Entities;

public class BatchEntity
{
    public int Id { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? FinishDate { get; set; }
    public string Status { get; set; }
    public int QtdyFiles { get; set; }
    public int QtdyFilesProcessed { get; set; }
}
