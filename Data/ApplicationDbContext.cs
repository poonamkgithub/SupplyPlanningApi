using Microsoft.EntityFrameworkCore;
using SupplyPlanningApi.Models;

namespace SupplyPlanningApi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<SupplyPlan> SupplyPlans => Set<SupplyPlan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var plan = modelBuilder.Entity<SupplyPlan>();
        plan.HasKey(x => x.Id);
        plan.Property(x => x.ProductCode).HasMaxLength(50).IsRequired();
        plan.Property(x => x.DemandQuantity).HasPrecision(18, 2);
        plan.Property(x => x.AvailableInventory).HasPrecision(18, 2);
        plan.Property(x => x.IncomingSupply).HasPrecision(18, 2);
        plan.Property(x => x.SafetyStock).HasPrecision(18, 2);
        plan.Property(x => x.TotalAvailableSupply).HasPrecision(18, 2);
        plan.Property(x => x.NetSupplyRequired).HasPrecision(18, 2);
        plan.Property(x => x.Recommendation).HasMaxLength(100).IsRequired();
    }
}
