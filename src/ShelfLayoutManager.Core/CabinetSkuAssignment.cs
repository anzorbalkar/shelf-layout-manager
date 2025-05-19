namespace ShelfLayoutManager.Core;

public record class CabinetSkuAssignment
{
    public long CabinetNumber { get; set; }

    public string JanCode { get; set; } = "";

    public long FromRowNumber { get; set; }

    public long FromLaneNumber { get; set; }

    public long ToRowNumber { get; set; }

    public long ToLaneNumber { get; set; }
}