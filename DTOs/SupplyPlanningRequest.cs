using System.ComponentModel.DataAnnotations;

namespace SupplyPlanningApi.DTOs;

public class SupplyPlanningRequest
{
    [Required, StringLength(50, MinimumLength = 1)]
    public string ProductCode { get; set; } = string.Empty;

    [Range(typeof(decimal), "0", "1000000000")]
    public decimal DemandQuantity { get; set; }

    [Range(typeof(decimal), "0", "1000000000")]
    public decimal AvailableInventory { get; set; }

    [Range(typeof(decimal), "0", "1000000000")]
    public decimal IncomingSupply { get; set; }

    [Range(typeof(decimal), "0", "1000000000")]
    public decimal SafetyStock { get; set; }
}
