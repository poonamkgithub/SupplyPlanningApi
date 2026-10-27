using Microsoft.EntityFrameworkCore;
using SupplyPlanningApi.Data;
using SupplyPlanningApi.DTOs;
using SupplyPlanningApi.Models;

namespace SupplyPlanningApi.Services;

public class SupplyPlanningService : ISupplyPlanningService
{
    private readonly ApplicationDbContext _db;

    public SupplyPlanningService(ApplicationDbContext db) => _db = db;

    public async Task<SupplyPlanningResponse> CreateAsync(
        SupplyPlanningRequest request, CancellationToken cancellationToken)
    {
        var plan = BuildPlan(request);
        _db.SupplyPlans.Add(plan);
        await _db.SaveChangesAsync(cancellationToken);
        return ToResponse(plan);
    }

    public async Task<List<SupplyPlanningResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        var plans = await _db.SupplyPlans
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        return plans.Select(ToResponse).ToList();
    }

    public async Task<SupplyPlanningResponse?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var plan = await _db.SupplyPlans.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return plan is null ? null : ToResponse(plan);
    }

    public async Task<SupplyPlanningResponse?> UpdateAsync(
        int id, SupplyPlanningRequest request, CancellationToken cancellationToken)
    {
        var plan = await _db.SupplyPlans
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (plan is null) return null;

        var recalculated = BuildPlan(request);
        plan.ProductCode = recalculated.ProductCode;
        plan.DemandQuantity = recalculated.DemandQuantity;
        plan.AvailableInventory = recalculated.AvailableInventory;
        plan.IncomingSupply = recalculated.IncomingSupply;
        plan.SafetyStock = recalculated.SafetyStock;
        plan.TotalAvailableSupply = recalculated.TotalAvailableSupply;
        plan.NetSupplyRequired = recalculated.NetSupplyRequired;
        plan.Recommendation = recalculated.Recommendation;

        await _db.SaveChangesAsync(cancellationToken);
        return ToResponse(plan);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var plan = await _db.SupplyPlans.FindAsync(new object[] { id }, cancellationToken);
        if (plan is null) return false;

        _db.SupplyPlans.Remove(plan);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static SupplyPlan BuildPlan(SupplyPlanningRequest request)
    {
        // Business rule: net requirement = max(0, demand + safety stock - inventory - incoming supply).
        var totalSupply = request.AvailableInventory + request.IncomingSupply;
        var required = Math.Max(0m, request.DemandQuantity + request.SafetyStock - totalSupply);

        return new SupplyPlan
        {
            ProductCode = request.ProductCode.Trim().ToUpperInvariant(),
            DemandQuantity = request.DemandQuantity,
            AvailableInventory = request.AvailableInventory,
            IncomingSupply = request.IncomingSupply,
            SafetyStock = request.SafetyStock,
            TotalAvailableSupply = totalSupply,
            NetSupplyRequired = required,
            Recommendation = required > 0
                ? "Additional supply required"
                : "Sufficient supply available",
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    private static SupplyPlanningResponse ToResponse(SupplyPlan plan) => new()
    {
        Id = plan.Id,
        ProductCode = plan.ProductCode,
        DemandQuantity = plan.DemandQuantity,
        AvailableInventory = plan.AvailableInventory,
        IncomingSupply = plan.IncomingSupply,
        SafetyStock = plan.SafetyStock,
        TotalAvailableSupply = plan.TotalAvailableSupply,
        NetSupplyRequired = plan.NetSupplyRequired,
        Recommendation = plan.Recommendation,
        CreatedAtUtc = plan.CreatedAtUtc
    };
}
