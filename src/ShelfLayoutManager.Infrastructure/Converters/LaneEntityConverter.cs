using ShelfLayoutManager.Core;

namespace ShelfLayoutManager.Infrastructure.Converters;

/// <inheritdoc>
public class LaneEntityConverter : ILaneEntityConverter
{
    public Lane? ConvertToLane(LaneEntity? laneEntity)
    {
        if (laneEntity is null)
        {
            return null;
        }

        return new Lane()
        {
            Id = laneEntity.Id,
            Number = laneEntity.Number,
            JanCode = laneEntity.JanCode,
            Quantity = laneEntity.Quantity,
            PositionX = laneEntity.X
        };
    }

    public LaneEntity? ConvertToLaneEntity(Lane? lane)
    {
        if (lane is null)
        {
            return null;
        }

        return new LaneEntity()
        {
            Id = lane.Id,
            Number = lane.Number,
            JanCode = lane.JanCode,
            Quantity = lane.Quantity,
            X = lane.PositionX
        };
    }
}