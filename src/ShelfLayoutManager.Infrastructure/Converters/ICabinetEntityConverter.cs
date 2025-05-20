using ShelfLayoutManager.Core;

namespace ShelfLayoutManager.Infrastructure.Converters;

/// <summary>
/// Provides methods to convert to and from <see cref="CabinetEntity"/> and <see cref="Cabinet"/>.  
/// </summary>
public interface ICabinetEntityConverter
{
    /// <summary>
    /// Converts the given <see cref="CabinetEntity"/> to <see cref="Cabinet"/>.
    /// </summary>
    Cabinet? ConvertToCabinet(CabinetEntity? cabinetEntity);

    /// <summary>
    /// Converts the given <see cref="Cabinet"/> to <see cref="CabinetEntity"/>.
    /// </summary>
    CabinetEntity? ConvertToCabinetEntity(Cabinet? cabinet);
}