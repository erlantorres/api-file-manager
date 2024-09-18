
using FileManager.Api.Data.Entities;
using FileManager.Api.Dtos;

namespace FileManager.Api.Profiles;

public static class BatchProfile
{
    public static BatchDto ParseToDto(this BatchEntity? entity)
    {
        if (entity == null)
        {
            return new();
        }

        return new BatchDto
        {
            Id = entity.Id,
            CreateDate = entity.CreateDate,
            FinishDate = entity.FinishDate,
            QtdyFiles = entity.QtdyFiles,
            QtdyFilesProcessed = entity.QtdyFilesProcessed
        };
    }
}
