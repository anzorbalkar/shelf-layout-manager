using ShelfLayoutManager.Core;

namespace ShelfLayoutManager.Infrastructure.Converters;

/// <inheritdoc>
public class SkuEntityConverter : ISkuEntityConverter
{
    public Sku? ConvertToSku(SkuEntity? skuEntity)
    {
        if (skuEntity is null)
        {
            return null;
        }

        return new Sku()
        {
            Id = skuEntity.Id,
            JanCode = skuEntity.JanCode,
            NameJp = skuEntity.NameJp,
            NameEn = skuEntity.NameEn,
            Volume = skuEntity.Volume,
            Size = new Size3() { Width = skuEntity.Width, Depth = skuEntity.Depth, Height = skuEntity.Height },
            Shape = skuEntity.Shape,
            ImageUrl = skuEntity.ImageUrl,
            RegistrationDate = skuEntity.RegistrationDate
        };
    }
    public SkuEntity? ConvertToSkuEntity(Sku? sku)
    {
        if (sku is null)
        {
            return null;
        }

        return new SkuEntity()
        {
            Id = sku.Id,
            JanCode = sku.JanCode,
            NameJp = sku.NameJp,
            NameEn = sku.NameEn,
            Volume = sku.Volume,
            Width = sku.Size.Width,
            Depth = sku.Size.Depth,
            Height = sku.Size.Height,
            Shape = sku.Shape,
            ImageUrl = sku.ImageUrl,
            RegistrationDate = sku.RegistrationDate
        };
    }
}