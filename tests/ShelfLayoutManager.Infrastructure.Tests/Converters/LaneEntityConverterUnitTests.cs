using AutoFixture;
using FluentAssertions;
using ShelfLayoutManager.Core;
using ShelfLayoutManager.Infrastructure.Converters;

namespace ShelfLayoutManager.Infrastructure.Tests.Converters;

public class LaneEntityConverterUnitTests
{
    private readonly ILaneEntityConverter _laneEntityConverter = new LaneEntityConverter();

    private readonly Fixture _fixture = new();

    public LaneEntityConverterUnitTests()
    {
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public void ConvertToLane_WhenNotNull_ReturnsMappedLane()
    {
        LaneEntity laneEntity = _fixture.Create<LaneEntity>();

        Lane? lane = _laneEntityConverter.ConvertToLane(laneEntity);

        lane.Should().NotBeNull();
        lane.Id.Should().Be(laneEntity.Id);
        lane.Number.Should().Be(laneEntity.Number);
        lane.JanCode.Should().Be(laneEntity.JanCode);
        lane.Quantity.Should().Be(laneEntity.Quantity);
        lane.PositionX.Should().Be(laneEntity.X);
    }

    [Fact]
    public void ConvertToLane_WhenNull_ReturnsNull()
    {
        _laneEntityConverter.ConvertToLane(null).Should().BeNull();
    }

    [Fact]
    public void ConvertToLaneEntity_WhenNotNull_ReturnsMappedLane()
    {
        Lane lane = _fixture.Create<Lane>();

        LaneEntity? laneEntity = _laneEntityConverter.ConvertToLaneEntity(lane);

        laneEntity.Should().NotBeNull();
        laneEntity.Id.Should().Be(lane.Id);
        laneEntity.Number.Should().Be(lane.Number);
        laneEntity.JanCode.Should().Be(lane.JanCode);
        laneEntity.Quantity.Should().Be(lane.Quantity);
        laneEntity.X.Should().Be(lane.PositionX);
    }

    [Fact]
    public void ConvertToLaneEntity_WhenNull_ReturnsNull()
    {
        _laneEntityConverter.ConvertToLaneEntity(null).Should().BeNull();
    }
}