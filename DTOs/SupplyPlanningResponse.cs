namespace SupplyPlanningApi.DTOs;

public class SupplyPlanningResponse
{
    public int Id { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public decimal DemandQuantity { get; set; }
    public decimal AvailableInventory { get; set; }
    public decimal IncomingSupply { get; set; }
    public decimal SafetyStock { get; set; }
    public decimal TotalAvailableSupply { get; set; }
    public decimal NetSupplyRequired { get; set; }
    public string Recommendation { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
}
