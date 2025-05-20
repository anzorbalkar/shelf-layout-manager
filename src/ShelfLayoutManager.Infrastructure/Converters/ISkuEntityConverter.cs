using ShelfLayoutManager.Core;

namespace ShelfLayoutManager.Infrastructure.Converters;

/// <summary>
/// Provides methods to convert to and from <see cref="SkuEntity"/> and <see cref="Sku"/>.  
/// </summary>
public interface ISkuEntityConverter
{
    /// <summary>
    /// Converts the given <see cref="SkuEntity"/> to <see cref="Sku"/>.
    /// </summary>
    Sku? ConvertToSku(SkuEntity? skuEntity);

    /// <summary>
    /// Converts the given <see cref="Sku"/> to <see cref="SkuEntity"/>.
    /// </summary>
    SkuEntity? ConvertToSkuEntity(Sku? sku);
}