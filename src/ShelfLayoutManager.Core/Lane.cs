namespace ShelfLayoutManager.Core;

public record class Lane
{
    public long Id { get; set; }

    public long Number { get; set; }

    public string? JanCode { get; set; }

    public int Quantity { get; set; }

    public float PositionX { get; set; }
}