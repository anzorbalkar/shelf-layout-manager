using System.Text.Json;
using FluentResults;
using ShelfLayoutManager.Core;

namespace ShelfLayoutManager.Infrastructure.Ingestion;

/// <inheritdoc>
public class ShelfFileParser : IShelfFileParser
{
    /// <summary>
    /// Reflects the same shape as the `shelf.json` file for easy one line JSON parsing.
    /// </summary>
    /// <param name="Cabinets"></param>
    record ShelfFile(List<Cabinet>? Cabinets);

    public async Task<Result<IList<Cabinet>>> Parse(Stream stream, CancellationToken ct)
    {
        using StreamReader reader = new(stream);
        string rawJsonContent = await reader.ReadToEndAsync(ct);

        ShelfFile? shelfFile = null;

        try
        {
            shelfFile = JsonSerializer.Deserialize<ShelfFile>(rawJsonContent);
        }
        catch (JsonException jsonEx)
        {
            return Result.Fail(new Error("Invalid JSON format.").CausedBy(jsonEx));
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Unexpected error while parsing.").CausedBy(ex));
        }

        return Result.Ok((IList<Cabinet>)(shelfFile?.Cabinets ?? []));
    }
}