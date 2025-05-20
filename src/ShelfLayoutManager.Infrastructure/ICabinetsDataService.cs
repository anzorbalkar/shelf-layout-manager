using FluentResults;
using ShelfLayoutManager.Core;

namespace ShelfLayoutManager.Infrastructure;

/// <summary>
/// Provides methods to support CRUD and other database operations for <see cref="Cabinet"/>'s.
/// </summary>
public interface ICabinetsDataService
{
    Task<Result<Cabinet?>> GetAsync(long id, bool includeRows, bool includeLanes, CancellationToken ct);

    Task<Result<IList<Cabinet>>> ListAsync(bool includeRows, bool includeLanes, CancellationToken ct);

    Task<Result> AssignSkuAsync(CabinetSkuAssignment cabinetSkuAssignment, CancellationToken ct);
}