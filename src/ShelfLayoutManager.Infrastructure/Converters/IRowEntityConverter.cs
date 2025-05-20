using ShelfLayoutManager.Core;

namespace ShelfLayoutManager.Infrastructure.Converters;

/// <summary>
/// Provides methods to convert to and from <see cref="RowEntity"/> and <see cref="Row"/>.  
/// </summary>
public interface IRowEntityConverter
{
    /// <summary>
    /// Converts the given <see cref="RowEntity"/> to <see cref="Row"/>.
    /// </summary>
    Row? ConvertToRow(RowEntity? rowEntity);

    /// <summary>
    /// Converts the given <see cref="Row"/> to <see cref="RowEntity"/>.
    /// </summary>
    RowEntity? ConvertToRowEntity(Row? row);
}