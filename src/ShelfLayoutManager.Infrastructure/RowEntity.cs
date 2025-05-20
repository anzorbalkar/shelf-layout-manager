namespace ShelfLayoutManager.Infrastructure;

public record class RowEntity
{
    public long Id { get; set; }

    public long Number { get; set; }

    public CabinetEntity? Cabinet { get; set; }

    public long? CabinetId { get; set; }

    public List<LaneEntity> Lanes { get; set; } = [];

    public float Z { get; set; }

    public float Height { get; set; }
}