
using api.Domain.FileServices.Data.Entities;
using api.Domain.FileServices.Dtos;

namespace api.Domain.FileServices.Profiles;

public static class FileProfile
{
    public static FileDto ParseToDto(this FileEntity entity)
    {
        if (entity == null)
        {
            return new();
        }

        return new FileDto
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
}
