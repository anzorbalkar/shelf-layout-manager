using FluentResults;
using Microsoft.AspNetCore.Mvc;
using ShelfLayoutManager.Infrastructure.Ingestion;

namespace ShelfLayoutManager.Web.Controllers;

[ApiController]
[Route("v1/utils")]
public class UtilsController(IIngestionService ingestionService) : ControllerBase
{
    private readonly IIngestionService _ingestionService = ingestionService;

    [HttpPost("ingest-sku-file")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> IngestSkuFileAsync(IFormFile file, CancellationToken ct)
    {
        if (file == null)
        {
            return BadRequest("No file was uploaded.");
        }

        if (file.Length == 0)
        {
            return BadRequest("The uploaded file is empty.");
        }

        if (!Path.GetExtension(file.FileName).Equals(".csv", StringComparison.InvariantCultureIgnoreCase))
        {
            return BadRequest("Invalid file type. Only .csv files are allowed.");
        }

        Result ingestionResult = await _ingestionService.IngestSkuFileAsync(file.OpenReadStream(), ct);

        if (ingestionResult.IsFailed)
        {
            return BadRequest(ingestionResult.Errors);
        }

        return Ok();
    }

    [HttpPost("ingest-shelf-file")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> IngestShelfFileAsync(IFormFile file, CancellationToken ct)
    {
        if (file == null)
        {
            return BadRequest("No file was uploaded.");
        }

        if (file.Length == 0)
        {
            return BadRequest("The uploaded file is empty.");
        }

        if (!Path.GetExtension(file.FileName).Equals(".json", StringComparison.InvariantCultureIgnoreCase))
        {
            return BadRequest("Invalid file type. Only .json files are allowed.");
        }

        Result ingestionResult = await _ingestionService.IngestShelfFileAsync(file.OpenReadStream(), ct);

        if (ingestionResult.IsFailed)
        {
            return BadRequest(ingestionResult.Errors);
        }

        return Ok();
    }
}
