using ShelfLayoutManager.Core;

namespace ShelfLayoutManager.Infrastructure;

/// <summary>
/// Provides methods to support CRUD and other database operations for <see cref="Sku"/>'s.
/// </summary>
public interface ISkusDataService
{
    Task<Sku?> GetAsync(long id, CancellationToken ct);

    Task<IList<Sku>> ListAsync(CancellationToken ct);
}