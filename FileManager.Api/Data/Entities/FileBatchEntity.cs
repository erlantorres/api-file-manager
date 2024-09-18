
namespace FileManager.Api.Data.Entities;

public class FileBatchEntity
{
    public int Id { get; set; }
    public int BatchId { get; set; }
    public DateTime Start { get; set; }
    public DateTime? Finish { get; set; }
    public string Status { get; set; }
    public string FileName { get; set; }
    public string Message { get; set; }
}
