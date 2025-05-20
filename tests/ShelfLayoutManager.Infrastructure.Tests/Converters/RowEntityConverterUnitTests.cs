using AutoFixture;
using FluentAssertions;
using Moq;
using ShelfLayoutManager.Core;
using ShelfLayoutManager.Infrastructure.Converters;

namespace ShelfLayoutManager.Infrastructure.Tests.Converters;

public class RowEntityConverterUnitTests
{
    private readonly Mock<ILaneEntityConverter> _mockLaneEntityConverter = new();

    private readonly IRowEntityConverter _rowEntityConverter;

    private readonly Fixture _fixture = new();

    public RowEntityConverterUnitTests()
    {
        _rowEntityConverter = new RowEntityConverter(_mockLaneEntityConverter.Object);
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public void ConvertToRow_WhenNotNull_ReturnsMappedRow()
    {
        RowEntity rowEntity = _fixture.Create<RowEntity>();

        _mockLaneEntityConverter.Setup(lec => lec.ConvertToLane(It.IsAny<LaneEntity>())).Returns(new Lane());

        Row? row = _rowEntityConverter.ConvertToRow(rowEntity);

        row.Should().NotBeNull();
        row.Id.Should().Be(rowEntity.Id);
        row.Number.Should().Be(rowEntity.Number);
        row.Lanes.Should().HaveSameCount(rowEntity.Lanes);
        row.PositionZ.Should().Be(rowEntity.Z);
        row.Size.Should().Be(new Size3() { Height = rowEntity.Height });
    }

    [Fact]
    public void ConvertToRow_WhenNull_ReturnsNull()
    {
        _rowEntityConverter.ConvertToRow(null).Should().BeNull();
    }

    [Fact]
    public void ConvertToRowEntity_WhenNotNull_ReturnsMappedRow()
    {
        Row row = _fixture.Create<Row>();

        _mockLaneEntityConverter.Setup(lec => lec.ConvertToLaneEntity(It.IsAny<Lane>())).Returns(new LaneEntity());

        RowEntity? rowEntity = _rowEntityConverter.ConvertToRowEntity(row);

        rowEntity.Should().NotBeNull();
        rowEntity.Id.Should().Be(row.Id);
        rowEntity.Number.Should().Be(row.Number);
        rowEntity.Lanes.Should().HaveSameCount(row.Lanes);
        rowEntity.Z.Should().Be(row.PositionZ);
        rowEntity.Height.Should().Be(row.Size.Height);
    }

    [Fact]
    public void ConvertToRowEntity_WhenNull_ReturnsNull()
    {
        _rowEntityConverter.ConvertToRowEntity(null).Should().BeNull();
    }
}