using ShelfLayoutManager.Core;

namespace ShelfLayoutManager.Infrastructure.Converters;

/// <inheritdoc>
public class RowEntityConverter(ILaneEntityConverter laneEntityConverter) : IRowEntityConverter
{
    private readonly ILaneEntityConverter _laneEntityConverter = laneEntityConverter;

    public Row? ConvertToRow(RowEntity? rowEntity)
    {
        if (rowEntity is null)
        {
            return null;
        }

        return new Row()
        {
            Id = rowEntity.Id,
            Number = rowEntity.Number,
            Lanes = [.. rowEntity.Lanes.Select(_laneEntityConverter.ConvertToLane).OfType<Lane>()],
            PositionZ = rowEntity.Z,
            Size = new Size3() { Height = rowEntity.Height }
        };
    }

    public RowEntity? ConvertToRowEntity(Row? row)
    {
        if (row is null)
        {
            return null;
        }

        return new RowEntity()
        {
            Id = row.Id,
            Number = row.Number,
            Lanes = [.. row.Lanes.Select(_laneEntityConverter.ConvertToLaneEntity).OfType<LaneEntity>()],
            Z = row.PositionZ,
            Height = row.Size.Height
        };
    }
}