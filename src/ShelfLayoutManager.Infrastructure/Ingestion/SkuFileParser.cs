using FluentResults;
using ShelfLayoutManager.Core;

namespace ShelfLayoutManager.Infrastructure.Ingestion;

/// <inheritdoc>
public class SkuFileParser : ISkuFileParser
{
    public async Task<Result<IList<Sku>>> Parse(Stream stream, CancellationToken ct)
    {
        List<Sku> skus = [];

        using StreamReader reader = new(stream);

        int i = 0;
        string? line;
        while ((line = await reader.ReadLineAsync(ct)) != null)
        {
            string[] parts = line.Split(',');

            string? janCodePart = parts.ElementAtOrDefault(0);
            string? namePart = parts.ElementAtOrDefault(1);
            string? widthPart = parts.ElementAtOrDefault(2);
            string? depthPart = parts.ElementAtOrDefault(3);
            string? heightPart = parts.ElementAtOrDefault(4);
            string? imageUrlPart = parts.ElementAtOrDefault(5);
            string? volumePart = parts.ElementAtOrDefault(6);
            string? timestampPart = parts.ElementAtOrDefault(7);
            string? shapePart = parts.ElementAtOrDefault(8);

            // Ignore the first line since it's titles, ie, "JanCode,Name,X,Y,Z,ImageURL,Size,TimeStamp,Shape".
            if (i++ == 0)
            {
                continue;
            }

            // The name part is first the Japanese name, then the English name, separated by a comma.
            string[] nameParts = (namePart ?? "").Split('/');

            DateTimeOffset? registrationDate = null;
            SkuShape skuShape = SkuShape.Unspecified;

            if (!float.TryParse(widthPart, out float width))
            {
                return Result.Fail($"Failed while parsing width: '{widthPart}'.");
            }

            if (!float.TryParse(depthPart, out float depth))
            {
                return Result.Fail($"Failed while parsing depth: '{depthPart}'.");
            }

            if (!float.TryParse(heightPart, out float height))
            {
                return Result.Fail($"Failed while parsing height: '{heightPart}'.");
            }

            if (!float.TryParse(volumePart, out float volume))
            {
                return Result.Fail($"Failed while parsing volume: '{volumePart}'.");
            }

            if (long.TryParse(timestampPart, out long parsedTimestamp))
            {
                registrationDate = DateTimeOffset.FromUnixTimeSeconds(parsedTimestamp);
            }
            else
            {
                return Result.Fail($"Failed while parsing timestamp: '{timestampPart}'.");
            }

            if (Enum.TryParse<SkuShape>(shapePart, out SkuShape parsedSkuShape))
            {
                skuShape = parsedSkuShape;
            }
            else
            {
                return Result.Fail($"Failed while parsing shape: '{shapePart}'.");
            }

            skus.Add(new Sku()
            {
                JanCode = janCodePart ?? "",
                NameJp = nameParts.FirstOrDefault(),
                NameEn = nameParts.LastOrDefault(),
                Size = new Size3() { Width = width, Depth = depth, Height = height },
                ImageUrl = imageUrlPart,
                Volume = volume,
                RegistrationDate = registrationDate,
                Shape = skuShape
            });
        }

        return skus;
    }
}