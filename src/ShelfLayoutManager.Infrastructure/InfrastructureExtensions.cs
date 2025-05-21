using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShelfLayoutManager.Infrastructure.Converters;
using ShelfLayoutManager.Infrastructure.Ingestion;

namespace ShelfLayoutManager.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddDbContext<DbContext>(options => options.UseNpgsql(configuration.GetConnectionString("TxConnection")))
            .AddScoped<ICabinetEntityConverter, CabinetEntityConverter>()
            .AddScoped<ICabinetsDataService, CabinetsDataService>()
            .AddScoped<IIngestionService, IngestionService>()
            .AddScoped<ILaneEntityConverter, LaneEntityConverter>()
            .AddScoped<IRowEntityConverter, RowEntityConverter>()
            .AddScoped<IShelfFileParser, ShelfFileParser>()
            .AddScoped<ISkuEntityConverter, SkuEntityConverter>()
            .AddScoped<ISkuFileParser, SkuFileParser>()
            .AddScoped<ISkusDataService, SkusDataService>();
}