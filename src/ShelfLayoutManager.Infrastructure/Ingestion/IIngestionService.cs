using FluentResults;

namespace ShelfLayoutManager.Infrastructure.Ingestion;

/// <summary>
/// Provides utility methods for ingesting the `sku.json` and `shelf.json` files into the database.
/// </summary>
public interface IIngestionService
{
    /// <summary>
    /// Ingests the SKUs from the given `sku.json` file stream into the database. This method will first purge all 
    /// existing SKU data from the database.
    /// </summary>
    Task<Result> IngestSkuFileAsync(Stream fileStream, CancellationToken ct);

    /// <summary>
    /// Ingests the <see cref="Cabinet"/>'s , <see cref="Row"/>'s, and <see cref="Lane"/>'s from the given `shelf.json`
    /// file stream into the database. This method will first purge all shelf data from the database.
    /// </summary>
    Task<Result> IngestShelfFileAsync(Stream fileStream, CancellationToken ct);
}