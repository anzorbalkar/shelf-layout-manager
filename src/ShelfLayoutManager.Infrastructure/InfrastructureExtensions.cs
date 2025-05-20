using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShelfLayoutManager.Infrastructure.Converters;
using ShelfLayoutManager.Infrastructure.Ingestion;

namespace ShelfLayoutManager.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DbContext>(
            options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ICabinetEntityConverter, CabinetEntityConverter>();
        services.AddScoped<ICabinetsDataService, CabinetsDataService>();
        services.AddScoped<IIngestionService, IngestionService>();
        services.AddScoped<ILaneEntityConverter, LaneEntityConverter>();
        services.AddScoped<IRowEntityConverter, RowEntityConverter>();
        services.AddScoped<IShelfFileParser, ShelfFileParser>();
        services.AddScoped<ISkuEntityConverter, SkuEntityConverter>();
        services.AddScoped<ISkuFileParser, SkuFileParser>();
        services.AddScoped<ISkusDataService, SkusDataService>();

        return services;
    }
}