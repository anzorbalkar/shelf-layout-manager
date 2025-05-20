namespace ShelfLayoutManager.Infrastructure;

public record class CabinetEntity
{
    public long Id { get; set; }

    public long Number { get; set; }

    public List<RowEntity> Rows { get; set; } = [];

    public float X { get; set; }

    public float Y { get; set; }

    public float Z { get; set; }

    public float Width { get; set; }

    public float Depth { get; set; }

    public float Height { get; set; }
}