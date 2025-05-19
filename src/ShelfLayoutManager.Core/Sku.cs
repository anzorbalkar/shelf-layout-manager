namespace ShelfLayoutManager.Core;

public enum SkuShape
{
    Unspecified,
    Can,
    Bottle,
    Box
}

public record class Sku
{
    public long Id { get; set; }

    public string JanCode { get; set; } = "";

    public string? NameJp { get; set; }

    public string? NameEn { get; set; }

    public float Volume { get; set; }

    public Size3 Size { get; set; } = new();

    public SkuShape Shape { get; set; }

    public string? ImageUrl { get; set; }

    public DateTimeOffset? RegistrationDate { get; set; }
}