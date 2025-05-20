namespace ShelfLayoutManager.Infrastructure;

public record class LaneEntity
{
    public long Id { get; set; }

    public long Number { get; set; }

    public RowEntity? Row { get; set; }

    public long? RowId { get; set; }

    public string? JanCode { get; set; }

    public int Quantity { get; set; }

    public float X { get; set; }
}