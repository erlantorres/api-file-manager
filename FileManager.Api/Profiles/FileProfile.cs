
using FileManager.Api.Data.Entities;
using FileManager.Api.Dtos;

namespace FileManager.Api.Profiles;

public static class FileProfile
{
    public static FileContentDto ParseWithContentToDto(this FileEntity? entity)
    {
        if (entity == null)
        {
            return new();
        }

        return new()
        {
            Name = entity.Name,
            Operation = entity.Operation,
            CreateDate = entity.CreateDate,
            Status = entity.Status,
            UnTrustedName = entity.UnTrustedName,
            Size = entity.Size,
            Content = entity.Content
        };
    }

    public static FileDto ParseToDto(this FileEntity? entity)
    {
        if (entity == null)
        {
            return new();
        }

        return new()
        {
            Name = entity.Name,
            Operation = entity.Operation,
            CreateDate = entity.CreateDate,
            Status = entity.Status,
            UnTrustedName = entity.UnTrustedName,
            Size = entity.Size
        };
    }

    public static List<FileDto> ParseToDto(this IEnumerable<FileEntity> entities)
    {
        var dtos = new List<FileDto>();
        foreach (var entity in entities)
        {
            dtos.Add(entity.ParseToDto());
        }

        return dtos;
    }
}
