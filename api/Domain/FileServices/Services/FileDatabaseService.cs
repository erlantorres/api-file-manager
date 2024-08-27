
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using System.Net;
using api.Domain.FileServices.Data.Entities;
using api.Domain.FileServices.Data.Enums;
using api.Domain.FileServices.Data.Repositories.Interfaces;
using api.Domain.FileServices.Dtos;
using api.Domain.FileServices.Helpers;
using api.Domain.FileServices.Services.Interfaces;
using api.Domain.Shared.Helpers;

namespace api.Domain.FileServices.Services;

public class FileDatabaseService(
    ILogger<FileDatabaseService> logger,
    IFileRepository fileRepository) : IFileDatabaseService
{

    private static string GetErrorId { get { return Guid.NewGuid().ToString(); } }

    public async Task DeleteAsync(string operation, string fileName)
    {
        try
        {
            await fileRepository.DeleteAsync(new FileDeleteDto { Operation = operation, FileName = fileName });
        }
        catch (Exception ex)
        {
            var errorId = GetErrorId;
            logger.LogError(ex, $"Delete error id {errorId}: {ex.Message}");
            throw new Exception($"Error deleting a file! Please provide the ID {errorId} to support for assistance.");
        }
    }

    public Task<FileDto> DownloadAsync(string operation, string fileName)
    {
        throw new NotImplementedException();
    }

    public async Task UploadLargeFilesAsync(Stream stream, string contentType)
    {
        try
        {
            var boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(contentType));
            var reader = new MultipartReader(boundary, stream);

            var section = await reader.ReadNextSectionAsync();
            while (section != null)
            {
                var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition);
                if (hasContentDispositionHeader && MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                {
                    using var memoryStream = new MemoryStream();
                    await section.Body.CopyToAsync(memoryStream);
                    var fileBytes = memoryStream.ToArray();

                    await fileRepository.SaveAsync(new FileEntity
                    {
                        CreateDate = DateTimeHelper.DataHoraDeBrasilia,
                        Operation = contentDisposition.Name.Value,
                        Status = Enum.GetName(typeof(FileStatus), FileStatus.UPLOAD),
                        Name = WebUtility.HtmlEncode(contentDisposition.FileName.Value),
                        UnTrustedName = contentDisposition.FileName.Value,
                        Size = fileBytes.Length,
                        Content = fileBytes
                    });
                }

                section = await reader.ReadNextSectionAsync();
            }
        }
        catch (Exception ex)
        {
            var errorId = GetErrorId;
            logger.LogError(ex, $"UploadLargeFiles error id {errorId}: {ex.Message}");
            throw new Exception($"Error uploading large file! Please provide the ID {errorId} to support for assistance.");
        }
    }
}
