using Microsoft.EntityFrameworkCore;
using ShelfLayoutManager.Core;
using ShelfLayoutManager.Infrastructure.Converters;

namespace ShelfLayoutManager.Infrastructure;

/// <inheritdoc>
public class SkusDataService(DbContext dbContext, ISkuEntityConverter skuEntityConverter) : ISkusDataService
{
    private readonly DbContext _dbContext = dbContext;

    private readonly ISkuEntityConverter _skuEntityConverter = skuEntityConverter;

    public async Task<Sku?> GetAsync(long id, CancellationToken ct) =>
        _skuEntityConverter.ConvertToSku(await GetReadQueryable().Where(c => c.Id == id).FirstOrDefaultAsync(ct));

    public async Task<IList<Sku>> ListAsync(CancellationToken ct) =>
        (await GetReadQueryable().ToListAsync(ct)).Select(_skuEntityConverter.ConvertToSku).OfType<Sku>().ToArray();

    private IQueryable<SkuEntity> GetReadQueryable() => _dbContext.Skus.AsNoTracking();
}