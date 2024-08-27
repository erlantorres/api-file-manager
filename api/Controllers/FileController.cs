using api.Domain.Attributes;
using api.Domain.FileServices.Dtos;
using api.Domain.FileServices.Helpers;
using api.Domain.FileServices.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController(IFileDatabaseService fileDatabaseService) : ControllerBase
{
    [HttpPost]
    [Route("upload-large-files")]
    [MultipartFormData]
    [DisableFormValueModelBinding]
    public async Task<IActionResult> UploadLargeFileAsync()
    {
        try
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                return BadRequest($"The request couldn't be processed (Form without multipart content.).");
            }

            await fileDatabaseService.UploadLargeFilesAsync(Request.Body, Request.ContentType);
            return Ok("Files uploaded");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(string operation, string fileName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(operation))
            {
                return BadRequest($"{nameof(operation)} is required");
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                return BadRequest($"{nameof(fileName)} is required");
            }

            await fileDatabaseService.DeleteAsync(operation, fileName);
            return StatusCode(204, "File deleted");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<FileDto>> GetAsync(string operation, string fileName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(operation))
            {
                return BadRequest($"{nameof(operation)} is required");
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                return BadRequest($"{nameof(fileName)} is required");
            }

            var file = await fileDatabaseService.GetFileAsync(operation, fileName);
            if (file == null || string.IsNullOrWhiteSpace(file.Name))
            {
                return NotFound();
            }

            return Ok(file);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet]
    [Route("get-all")]
    public async Task<ActionResult<List<FileDto>>> GetAllAsync(string operation)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(operation))
            {
                return BadRequest($"{nameof(operation)} is required");
            }

            var files = await fileDatabaseService.GetAllFileAsync(operation);
            if (files == null || files.Count <= 0)
            {
                return NotFound();
            }

            return Ok(files);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
