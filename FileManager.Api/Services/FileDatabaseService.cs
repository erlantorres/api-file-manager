
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using System.Net;
using FileManager.Api.Data.Entities;
using FileManager.Api.Data.Enums;
using FileManager.Api.Data.Repositories.Interfaces;
using FileManager.Api.Dtos;
using FileManager.Api.Helpers;
using FileManager.Api.Services.Interfaces;
using FileManager.Api.Profiles;

namespace FileManager.Api.Services;

public class FileDatabaseService(
    ILogger<FileDatabaseService> logger,
    IFileRepository fileRepository) : IFileDatabaseService
{
    private static string GetErrorId { get { return Guid.NewGuid().ToString(); } }

    public async Task DeleteAsync(string operation, string fileName)
    {
        try
        {
            await fileRepository.DeleteAsync(operation, fileName);
        }
        catch (Exception ex)
        {
            var errorId = GetErrorId;
            logger.LogError(ex, $"DeleteAsync error id {errorId}: {ex.Message}");
            throw new Exception($"Error deleting a file! Please provide the ID {errorId} to support for assistance.");
        }
    }

    public async Task<FileContentDto> GetFileAsync(string operation, string fileName)
    {
        try
        {
            var file = await fileRepository.GetWithContentAsync(operation, fileName);
            return file.ParseWithContentToDto();
        }
        catch (Exception ex)
        {
            var errorId = GetErrorId;
            logger.LogError(ex, $"GetFileAsync error id {errorId}: {ex.Message}");
            throw new Exception($"Error getting file! Please provide the ID {errorId} to support for assistance.");
        }
    }

    public async Task<List<FileDto>> GetAllFileAsync(string operation)
    {
        try
        {
            var files = await fileRepository.GetAllAsync(operation);
            return files.ParseToDto();
        }
        catch (Exception ex)
        {
            var errorId = GetErrorId;
            logger.LogError(ex, $"GetAllFileAsync error id {errorId}: {ex.Message}");
            throw new Exception($"Error getting files! Please provide the ID {errorId} to support for assistance.");
        }
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
            logger.LogError(ex, $"UploadLargeFilesAsync error id {errorId}: {ex.Message}");
            throw new Exception($"Error uploading large file! Please provide the ID {errorId} to support for assistance.");
        }
    }

    public async Task UpdateFileStatusAsync(string operation, string fileName, FileStatus status)
    {
        try
        {
            await fileRepository.UpdateFileStatusAsync(operation, fileName, Enum.GetName(typeof(FileStatus), status));
        }
        catch (Exception ex)
        {
            var errorId = GetErrorId;
            logger.LogError(ex, $"UpdateFileStatusAsync error id {errorId}: {ex.Message}");
            throw new Exception($"Error updating the file! Please provide the ID {errorId} to support for assistance.");
        }
    }
}
