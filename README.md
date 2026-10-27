# SupplyPlanningApi

A demo ASP.NET Core 8 Web API for supply planning with SQL Server, Entity Framework Core, Swagger, CRUD endpoints, validation, and a simple supply-shortage business rule.

## Business rule

`NetSupplyRequired = max(0, DemandQuantity + SafetyStock - AvailableInventory - IncomingSupply)`

Example: demand 1,000 + safety stock 50 - inventory 200 - incoming supply 100 = **750 units required**.

This is a learning/demo rule, not a full enterprise planning engine. Real planning may also include dates, supplier lead times, capacity, minimum order quantities, allocations, and constraints.

## Requirements

- .NET 8 SDK
- SQL Server LocalDB (default connection) or a SQL Server instance you can access

## Run

1. Extract the ZIP.
2. Open a terminal in the `SupplyPlanningApi` folder.
3. If needed, edit `ConnectionStrings:DefaultConnection` in `appsettings.json`.
4. Run:

   ```bash
   dotnet restore
   dotnet run
   ```

5. Open the HTTPS Swagger URL printed in the terminal, then append `/swagger` if needed.

The demo calls `EnsureCreated()` on startup to create the database/table if absent. For production and evolving schemas, use EF Core migrations instead.

## Endpoints

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/supply-planning/plan` | Calculate and save a plan |
| GET | `/api/supply-planning` | List plans |
| GET | `/api/supply-planning/{id}` | Get a plan |
| PUT | `/api/supply-planning/{id}` | Update inputs and recalculate |
| DELETE | `/api/supply-planning/{id}` | Delete a plan |

## Sample POST body

```json
{
  "productCode": "VESSEL-A",
  "demandQuantity": 1000,
  "availableInventory": 200,
  "incomingSupply": 100,
  "safetyStock": 50
}
```

Expected calculation fields include `totalAvailableSupply: 300`, `netSupplyRequired: 750`, and `recommendation: "Additional supply required"`.

## Notes

- API input validation rejects blank product codes and negative quantities.
- Plans are stored in SQL Server.
- This ZIP contains source code; it has not been compiled against your local machine or database.
