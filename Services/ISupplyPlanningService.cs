using SupplyPlanningApi.DTOs;

namespace SupplyPlanningApi.Services;

public interface ISupplyPlanningService
{
    Task<SupplyPlanningResponse?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<List<SupplyPlanningResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task<SupplyPlanningResponse> CreateAsync(SupplyPlanningRequest request, CancellationToken cancellationToken);
    Task<SupplyPlanningResponse?> UpdateAsync(int id, SupplyPlanningRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}
