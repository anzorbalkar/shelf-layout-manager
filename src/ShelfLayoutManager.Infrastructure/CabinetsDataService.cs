using FluentResults;
using Microsoft.EntityFrameworkCore;
using ShelfLayoutManager.Core;
using ShelfLayoutManager.Infrastructure.Converters;

namespace ShelfLayoutManager.Infrastructure;

/// <inheritdoc>
public class CabinetsDataService(DbContext dbContext, ICabinetEntityConverter cabinetEntityConverter)
    : ICabinetsDataService
{
    private readonly DbContext _dbContext = dbContext;

    private readonly ICabinetEntityConverter _cabinetEntityConverter = cabinetEntityConverter;

    public async Task<Result<Cabinet?>> GetAsync(long id, bool includeRows, bool includeLanes, CancellationToken ct) =>
        _cabinetEntityConverter.ConvertToCabinet(
            await GetReadQueryable(includeRows, includeLanes).Where(c => c.Id == id).FirstOrDefaultAsync(ct));

    public async Task<Result<IList<Cabinet>>> ListAsync(bool includeRows, bool includeLanes, CancellationToken ct) =>
        (await GetReadQueryable(includeRows, includeLanes).ToListAsync(ct))
            .Select(_cabinetEntityConverter.ConvertToCabinet).OfType<Cabinet>().ToArray();

    public async Task<Result> AssignSkuAsync(CabinetSkuAssignment cabinetSkuAssignment, CancellationToken ct)
    {
        // TODO(abalkar): Add validation for given assignment.

        bool getFromLane = cabinetSkuAssignment.FromRowNumber != 0 || cabinetSkuAssignment.FromLaneNumber != 0;
        bool getToLane = cabinetSkuAssignment.ToRowNumber != 0 || cabinetSkuAssignment.ToLaneNumber != 0;

        LaneEntity? fromLaneEntity = null;
        LaneEntity? toLaneEntity = null;

        if (getFromLane)
        {
            fromLaneEntity = await GetLane(
                cabinetSkuAssignment.CabinetNumber,
                cabinetSkuAssignment.FromRowNumber,
                cabinetSkuAssignment.FromLaneNumber,
                ct);

            if (fromLaneEntity is null)
            {
                return Result.Fail("Could not find from lane with given numbers.");
            }
        }

        if (getToLane)
        {
            toLaneEntity = await GetLane(
                cabinetSkuAssignment.CabinetNumber,
                cabinetSkuAssignment.ToRowNumber,
                cabinetSkuAssignment.ToLaneNumber,
                ct);

            if (toLaneEntity is null)
            {
                return Result.Fail("Could not find to lane with given numbers.");
            }
        }

        bool removingSku = getFromLane && !getToLane;
        bool assigningSku = !getFromLane && getToLane;
        bool movingSku = getFromLane && getToLane;

        if (removingSku)
        {
            fromLaneEntity!.JanCode = null;
        }
        else if (assigningSku)
        {
            toLaneEntity!.JanCode = cabinetSkuAssignment.JanCode;
        }
        else if (movingSku)
        {
            toLaneEntity!.JanCode = fromLaneEntity!.JanCode;
            toLaneEntity!.Quantity = fromLaneEntity!.Quantity;

            fromLaneEntity.JanCode = null;
        }

        await _dbContext.SaveChangesAsync(ct);

        return await Task.FromResult<Result>(Result.Ok());
    }

    private IQueryable<CabinetEntity> GetReadQueryable(bool includeRows, bool includeLanes)
    {
        IQueryable<CabinetEntity> queryableCabinets = _dbContext.Cabinets.AsNoTracking();

        if (includeRows)
        {
            if (includeLanes)
            {
                queryableCabinets =
                    queryableCabinets
                        .Include(ce => ce.Rows.OrderBy(row => row.Z))
                        .ThenInclude(re => re.Lanes.OrderBy(lane => lane.X));
            }
            else
            {
                queryableCabinets = queryableCabinets.Include(ce => ce.Rows.OrderBy(row => row.Z));
            }
        }

        return queryableCabinets;
    }

    private async Task<LaneEntity?> GetLane(
        long cabinetNumber, long rowNumber, long laneNumber, CancellationToken ct) =>
        await _dbContext.Lanes.Include(lane => lane.Row).ThenInclude(row => row!.Cabinet).Where(lane =>
                lane.Row!.Cabinet!.Number == cabinetNumber &&
                lane.Row.Number == rowNumber &&
                lane.Number == laneNumber).FirstOrDefaultAsync(ct);

}