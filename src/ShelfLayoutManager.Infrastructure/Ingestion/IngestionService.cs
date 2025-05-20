using FluentResults;
using Microsoft.EntityFrameworkCore;
using ShelfLayoutManager.Core;
using ShelfLayoutManager.Infrastructure.Converters;

namespace ShelfLayoutManager.Infrastructure.Ingestion;

/// <inheritdoc/>
public class IngestionService(
    DbContext dbContext,
    ISkuFileParser skuFileParser,
    IShelfFileParser shelfFileParser,
    ISkuEntityConverter skuEntityConverter,
    ICabinetEntityConverter cabinetEntityConverter) : IIngestionService
{
    private readonly DbContext _dbContext = dbContext;

    private readonly ISkuFileParser _skuFileParser = skuFileParser;

    private readonly IShelfFileParser _shelfFileParser = shelfFileParser;

    private readonly ISkuEntityConverter _skuEntityConverter = skuEntityConverter;

    private readonly ICabinetEntityConverter _cabinetEntityConverter = cabinetEntityConverter;

    public async Task<Result> IngestSkuFileAsync(Stream fileStream, CancellationToken ct)
    {
        // TODO(abalkar): Use transaction here.

        Result<IList<Sku>> skusResult = await _skuFileParser.Parse(fileStream, ct);

        await _dbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE public.\"Skus\"", ct);

        _dbContext.Skus.AddRange(skusResult.Value.Select(_skuEntityConverter.ConvertToSkuEntity).OfType<SkuEntity>());

        await _dbContext.SaveChangesAsync(ct);

        return skusResult.ToResult();
    }

    public async Task<Result> IngestShelfFileAsync(Stream fileStream, CancellationToken ct)
    {
        // TODO(abalkar): Use transaction here.

        Result<IList<Cabinet>> cabinetsResult = await _shelfFileParser.Parse(fileStream, ct);

        await _dbContext.Database.ExecuteSqlRawAsync(
            "TRUNCATE TABLE public.\"Lanes\", public.\"Rows\", public.\"Cabinets\"", ct);

        _dbContext.Cabinets.AddRange(
            cabinetsResult.Value.Select(_cabinetEntityConverter.ConvertToCabinetEntity).OfType<CabinetEntity>());

        await _dbContext.SaveChangesAsync(ct);

        return cabinetsResult.ToResult();
    }
}