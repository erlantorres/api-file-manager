namespace FileManager.Api.Data.Enums;

public enum FileStatus
{
    UPLOAD,
    PROCESSING,
    PROCESSING_FAILED,
    PROCESSED,
    UNDEFINED
}

public static class FileStatusExtensions
{
    public static string GetName(this FileStatus status)
    {
        return Enum.GetName(typeof(FileStatus), status);
    }
}