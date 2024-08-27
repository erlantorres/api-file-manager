
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

public class FileDatabaseService(IFileRepository fileRepository) : IFileDatabaseService
{
    public Task Delete(string path)
    {
        throw new NotImplementedException();
    }

    public Task<FileDto> Download(string path)
    {
        throw new NotImplementedException();
    }

    public Task Upload(FileDto file)
    {
        throw new NotImplementedException();
    }

    public async Task UploadLargeFiles(Stream stream, string contentType)
    {
        var boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(contentType));
        var reader = new MultipartReader(boundary, stream)
        {
            HeadersCountLimit = 200,
            HeadersLengthLimit = 1024 * 1024 * 1024,
            BodyLengthLimit = 1024 * 1024 * 1024
        };

        var section = await reader.ReadNextSectionAsync();
        while (section != null)
        {
            var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition);
            if (hasContentDispositionHeader && MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
            {
                using (var memoryStream = new MemoryStream())
                {
                    await section.Body.CopyToAsync(memoryStream);
                    var fileBytes = memoryStream.ToArray();

                    await fileRepository.Save(new FileEntity
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
            }

            section = await reader.ReadNextSectionAsync();
        }
    }
}
