using FluentResults;
using ShelfLayoutManager.Core;

namespace ShelfLayoutManager.Infrastructure.Ingestion;

/// <summary>
/// Provides methods to parse the `sku.json` file.
/// </summary>
public interface ISkuFileParser
{
    /// <summary>
    /// Parses the given `sku.json` file stream into <see cref="Sku"/>'s.
    /// </summary>
    Task<Result<IList<Sku>>> Parse(Stream stream, CancellationToken ct);
}