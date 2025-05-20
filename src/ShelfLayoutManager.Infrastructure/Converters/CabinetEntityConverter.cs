using ShelfLayoutManager.Core;

namespace ShelfLayoutManager.Infrastructure.Converters;

/// <inheritdoc>
public class CabinetEntityConverter(IRowEntityConverter rowEntityConverter) : ICabinetEntityConverter
{
    private readonly IRowEntityConverter _rowEntityConverter = rowEntityConverter;

    public Cabinet? ConvertToCabinet(CabinetEntity? cabinetEntity)
    {
        if (cabinetEntity is null)
        {
            return null;
        }

        return new Cabinet()
        {
            Id = cabinetEntity.Id,
            Number = cabinetEntity.Number,
            Rows = [.. cabinetEntity.Rows.Select(_rowEntityConverter.ConvertToRow).OfType<Row>()],
            Position = new Position3() { X = cabinetEntity.X, Y = cabinetEntity.Y, Z = cabinetEntity.Z },
            Size = new Size3()
            {
                Width = cabinetEntity.Width,
                Depth = cabinetEntity.Depth,
                Height = cabinetEntity.Height
            }
        };
    }

    public CabinetEntity? ConvertToCabinetEntity(Cabinet? cabinet)
    {
        if (cabinet is null)
        {
            return null;
        }

        return new CabinetEntity()
        {
            Id = cabinet.Id,
            Number = cabinet.Number,
            Rows = [.. cabinet.Rows.Select(_rowEntityConverter.ConvertToRowEntity).OfType<RowEntity>()],
            X = cabinet.Position.X,
            Y = cabinet.Position.Y,
            Z = cabinet.Position.Z,
            Width = cabinet.Size.Width,
            Depth = cabinet.Size.Depth,
            Height = cabinet.Size.Height
        };
    }
}