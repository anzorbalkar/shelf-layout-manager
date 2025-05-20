using ShelfLayoutManager.Core;

namespace ShelfLayoutManager.Infrastructure;

public record class SkuEntity
{
    public long Id { get; set; }

    public string JanCode { get; set; } = "";

    public string? NameJp { get; set; }

    public string? NameEn { get; set; }

    public float Volume { get; set; }

    public float Width { get; set; }

    public float Depth { get; set; }

    public float Height { get; set; }

    public SkuShape Shape { get; set; }

    public string? ImageUrl { get; set; }

    public DateTimeOffset? RegistrationDate { get; set; }
}