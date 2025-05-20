using FluentResults;
using Microsoft.AspNetCore.Mvc;
using ShelfLayoutManager.Core;
using ShelfLayoutManager.Infrastructure;

namespace ShelfLayoutManager.Web.Controllers;

[ApiController]
[Route("v1/cabinets")]
public class CabinetsController(ICabinetsDataService cabinetsDataService)
    : ControllerBase
{
    private readonly ICabinetsDataService _cabinetsDataService = cabinetsDataService;

    [HttpGet("{id}")]
    public async Task<ActionResult<Cabinet?>> GetAsync(
        [FromRoute] long id,
        [FromQuery] bool includeRows = false,
        [FromQuery] bool includeLanes = false,
        CancellationToken ct = default)
    {
        Result<Cabinet?> cabinetResult = await _cabinetsDataService.GetAsync(id, includeRows, includeLanes, ct);

        if (cabinetResult.Value is null)
        {
            return NotFound();
        }

        return cabinetResult.Value;
    }

    [HttpGet]
    public async Task<IList<Cabinet>> ListAsync(
        [FromQuery] bool includeRows = false, [FromQuery] bool includeLanes = false, CancellationToken ct = default)
            => (await _cabinetsDataService.ListAsync(includeRows, includeLanes, ct)).Value;

    [HttpPost("-/assign-sku")]
    public async Task<ActionResult> AssignSkuAsync(CabinetSkuAssignment cabinetSkuAssignment, CancellationToken ct)
    {
        await _cabinetsDataService.AssignSkuAsync(cabinetSkuAssignment, ct);

        return Ok();
    }
}
