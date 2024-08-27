
namespace api.Domain.FileServices.Dtos;

public class FileDto
{
    public string Name { get; set; }
    public string Operation { get; set; }
    public DateTime CreateDate { get; set; }
    public string Status { get; set; }
    public string UnTrustedName { get; set; }
    public long Size { get; set; }
    public byte[] Content { get; set; }
}
