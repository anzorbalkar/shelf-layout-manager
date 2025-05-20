using Microsoft.EntityFrameworkCore;

namespace ShelfLayoutManager.Infrastructure;

/// <summary>
/// The application database context.
/// </summary>
public class DbContext(DbContextOptions<DbContext> options) : Microsoft.EntityFrameworkCore.DbContext(options)
{
    public DbSet<SkuEntity> Skus { get; set; } = null!;

    public DbSet<CabinetEntity> Cabinets { get; set; } = null!;

    public DbSet<RowEntity> Rows { get; set; } = null!;

    public DbSet<LaneEntity> Lanes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SkuEntity>(skuEntity => skuEntity.HasKey(a => a.Id));
        modelBuilder.Entity<SkuEntity>().Property(skuEntity => skuEntity.Shape).HasConversion<string>();

        modelBuilder.Entity<CabinetEntity>(cabinetEntity => cabinetEntity.HasKey(a => a.Id));
        modelBuilder.Entity<RowEntity>(rowEntity => rowEntity.HasKey(a => a.Id));
        modelBuilder.Entity<LaneEntity>(laneEntity => laneEntity.HasKey(a => a.Id));

        modelBuilder.Entity<CabinetEntity>()
            .HasMany(e => e.Rows)
            .WithOne(e => e.Cabinet)
            .HasForeignKey(e => e.CabinetId)
            .IsRequired(false);

        modelBuilder.Entity<RowEntity>()
            .HasMany(e => e.Lanes)
            .WithOne(e => e.Row)
            .HasForeignKey(e => e.RowId)
            .IsRequired(false);
    }
}