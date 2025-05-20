using AutoFixture;
using FluentAssertions;
using ShelfLayoutManager.Core;
using ShelfLayoutManager.Infrastructure.Converters;

namespace ShelfLayoutManager.Infrastructure.Tests.Converters;

public class SkuEntityConverterUnitTests
{
    private readonly ISkuEntityConverter _skuEntityConverter = new SkuEntityConverter();

    private readonly Fixture _fixture = new();

    public SkuEntityConverterUnitTests()
    {
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public void ConvertToSku_WhenNotNull_ReturnsMappedSku()
    {
        SkuEntity skuEntity = _fixture.Create<SkuEntity>();

        Sku? sku = _skuEntityConverter.ConvertToSku(skuEntity);

        sku.Should().NotBeNull();
        sku.Id.Should().Be(skuEntity.Id);
        sku.JanCode.Should().Be(skuEntity.JanCode);
        sku.NameJp.Should().Be(skuEntity.NameJp);
        sku.NameEn.Should().Be(skuEntity.NameEn);
        sku.Volume.Should().Be(skuEntity.Volume);
        sku.Size.Should().Be(
            new Size3() { Width = skuEntity.Width, Depth = skuEntity.Depth, Height = skuEntity.Height });
        sku.Shape.Should().Be(skuEntity.Shape);
        sku.ImageUrl.Should().Be(skuEntity.ImageUrl);
        sku.RegistrationDate.Should().Be(skuEntity.RegistrationDate);
    }

    [Fact]
    public void ConvertToSku_WhenNull_ReturnsNull()
    {
        _skuEntityConverter.ConvertToSku(null).Should().BeNull();
    }

    [Fact]
    public void ConvertToSkuEntity_WhenNotNull_ReturnsMappedSku()
    {
        Sku sku = _fixture.Create<Sku>();

        SkuEntity? skuEntity = _skuEntityConverter.ConvertToSkuEntity(sku);

        skuEntity.Should().NotBeNull();
        skuEntity.Id.Should().Be(sku.Id);
        skuEntity.JanCode.Should().Be(sku.JanCode);
        skuEntity.NameJp.Should().Be(sku.NameJp);
        skuEntity.NameEn.Should().Be(sku.NameEn);
        skuEntity.Volume.Should().Be(sku.Volume);
        skuEntity.Width.Should().Be(sku.Size.Width);
        skuEntity.Depth.Should().Be(sku.Size.Depth);
        skuEntity.Height.Should().Be(sku.Size.Height);
        skuEntity.Shape.Should().Be(sku.Shape);
        skuEntity.ImageUrl.Should().Be(sku.ImageUrl);
        skuEntity.RegistrationDate.Should().Be(sku.RegistrationDate);
    }

    [Fact]
    public void ConvertToSkuEntity_WhenNull_ReturnsNull()
    {
        _skuEntityConverter.ConvertToSkuEntity(null).Should().BeNull();
    }
}