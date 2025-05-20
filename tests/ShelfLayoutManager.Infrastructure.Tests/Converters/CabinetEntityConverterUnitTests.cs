using AutoFixture;
using FluentAssertions;
using Moq;
using ShelfLayoutManager.Core;
using ShelfLayoutManager.Infrastructure.Converters;

namespace ShelfLayoutManager.Infrastructure.Tests.Converters;

public class CabinetEntityConverterUnitTests
{
    private readonly Mock<IRowEntityConverter> _mockRowEntityConverter = new();

    private readonly ICabinetEntityConverter _cabinetEntityConverter;

    private readonly Fixture _fixture = new();

    public CabinetEntityConverterUnitTests()
    {
        _cabinetEntityConverter = new CabinetEntityConverter(_mockRowEntityConverter.Object);
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public void ConvertToCabinet_WhenNotNull_ReturnsMappedCabinet()
    {
        CabinetEntity cabinetEntity = _fixture.Create<CabinetEntity>();

        _mockRowEntityConverter.Setup(lec => lec.ConvertToRow(It.IsAny<RowEntity>())).Returns(new Row());

        Cabinet? cabinet = _cabinetEntityConverter.ConvertToCabinet(cabinetEntity);

        cabinet.Should().NotBeNull();
        cabinet.Id.Should().Be(cabinetEntity.Id);
        cabinet.Number.Should().Be(cabinetEntity.Number);
        cabinet.Rows.Should().HaveSameCount(cabinetEntity.Rows);
        cabinet.Position.Should().Be(new Position3() { X = cabinetEntity.X, Y = cabinetEntity.Y, Z = cabinetEntity.Z });
        cabinet.Size.Should().Be(
            new Size3() { Width = cabinetEntity.Width, Depth = cabinetEntity.Depth, Height = cabinetEntity.Height });
    }

    [Fact]
    public void ConvertToCabinet_WhenNull_ReturnsNull()
    {
        _cabinetEntityConverter.ConvertToCabinet(null).Should().BeNull();
    }

    [Fact]
    public void ConvertToCabinetEntity_WhenNotNull_ReturnsMappedCabinet()
    {
        Cabinet cabinet = _fixture.Create<Cabinet>();

        _mockRowEntityConverter.Setup(lec => lec.ConvertToRowEntity(It.IsAny<Row>())).Returns(new RowEntity());

        CabinetEntity? cabinetEntity = _cabinetEntityConverter.ConvertToCabinetEntity(cabinet);

        cabinetEntity.Should().NotBeNull();
        cabinetEntity.Id.Should().Be(cabinet.Id);
        cabinetEntity.Number.Should().Be(cabinet.Number);
        cabinetEntity.Rows.Should().HaveSameCount(cabinet.Rows);
        cabinetEntity.X.Should().Be(cabinet.Position.X);
        cabinetEntity.Y.Should().Be(cabinet.Position.Y);
        cabinetEntity.Z.Should().Be(cabinet.Position.Z);
        cabinetEntity.Width.Should().Be(cabinet.Size.Width);
        cabinetEntity.Depth.Should().Be(cabinet.Size.Depth);
        cabinetEntity.Height.Should().Be(cabinet.Size.Height);
    }

    [Fact]
    public void ConvertToCabinetEntity_WhenNull_ReturnsNull()
    {
        _cabinetEntityConverter.ConvertToCabinetEntity(null).Should().BeNull();
    }
}