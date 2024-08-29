using api.Domain.FileServices.Dtos;
using api.Domain.FileServices.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileBatchManagerController(IFileBatchManagerService fileBatchManagerService) : ControllerBase
{
    [HttpPost]
    [Route("batch-process/files")]
    public async Task<ActionResult<BatchProcessResultDto>> BatchProcessAsync([FromBody] List<FileProcessDto> files)
    {
        try
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest("A list of files is required");
            }

            var result = await fileBatchManagerService.ProcessFilesAsync(files);
            return Ok(new BatchProcessResultDto { BatchId = result });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}