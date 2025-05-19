namespace ShelfLayoutManager.Core;

public record class Cabinet
{
    public long Id { get; set; }

    public long Number { get; set; }

    public IList<Row> Rows { get; set; } = Array.Empty<Row>();

    public Position3 Position { get; set; } = new();

    public Size3 Size { get; set; } = new();
}