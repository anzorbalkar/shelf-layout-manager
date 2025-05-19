namespace ShelfLayoutManager.Core;

public record class Row
{
    public long Id { get; set; }

    public long Number { get; set; }

    public IList<Lane> Lanes { get; set; } = Array.Empty<Lane>();

    public float PositionZ { get; set; }

    public Size3 Size { get; set; } = new();
}