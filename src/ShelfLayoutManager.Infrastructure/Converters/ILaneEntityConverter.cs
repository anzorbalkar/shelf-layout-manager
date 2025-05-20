using ShelfLayoutManager.Core;

namespace ShelfLayoutManager.Infrastructure.Converters;

/// <summary>
/// Provides methods to convert to and from <see cref="LaneEntity"/> and <see cref="Lane"/>.  
/// </summary>
public interface ILaneEntityConverter
{
    /// <summary>
    /// Converts the given <see cref="LaneEntity"/> to <see cref="Lane"/>.
    /// </summary>
    Lane? ConvertToLane(LaneEntity? laneEntity);

    /// <summary>
    /// Converts the given <see cref="Lane"/> to <see cref="LaneEntity"/>.
    /// </summary>
    LaneEntity? ConvertToLaneEntity(Lane? lane);
}