using Microsoft.AspNetCore.Mvc;
using ShelfLayoutManager.Core;
using ShelfLayoutManager.Infrastructure;

namespace ShelfLayoutManager.Web.Controllers;

[ApiController]
[Route("v1/skus")]
public class SkusController(ISkusDataService skusDataService) : ControllerBase
{
    private readonly ISkusDataService _skusDataService = skusDataService;

    [HttpGet("{id}")]
    public async Task<ActionResult<Sku?>> GetAsync([FromRoute] long id, CancellationToken ct)
    {
        Sku? sku = await _skusDataService.GetAsync(id, ct);

        if (sku is null)
        {
            return NotFound();
        }

        return sku;
    }

    [HttpGet]
    public async Task<IList<Sku>> ListAsync(CancellationToken ct) => await _skusDataService.ListAsync(ct);
}
