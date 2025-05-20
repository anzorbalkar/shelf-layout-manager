using FluentResults;
using ShelfLayoutManager.Core;

namespace ShelfLayoutManager.Infrastructure.Ingestion;

/// <summary>
/// Provides methods to parse the `shelf.json` file.
/// </summary>
public interface IShelfFileParser
{
    /// <summary>
    /// Parses the given `shelf.json` file stream into <see cref="Cabinet"/>'s, <see cref="Row"/>'s, and 
    /// <see cref="Lanes"/>'s.   
    /// </summary>
    Task<Result<IList<Cabinet>>> Parse(Stream stream, CancellationToken ct);
}