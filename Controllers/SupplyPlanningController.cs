using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupplyPlanningApi.DTOs;
using SupplyPlanningApi.Services;

namespace SupplyPlanningApi.Controllers;

[ApiController]
[Route("api/supply-planning")]
public class SupplyPlanningController : ControllerBase
{
    private readonly ISupplyPlanningService _service;

    public SupplyPlanningController(ISupplyPlanningService service) => _service = service;

    [HttpPost("plan")]
    [ProducesResponseType(typeof(SupplyPlanningResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SupplyPlanningResponse>> Create(
        [FromBody] SupplyPlanningRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _service.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (DbUpdateException)
        {
            return Problem("Could not save the supply plan to the database.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<SupplyPlanningResponse>>> GetAll(CancellationToken cancellationToken)
        => Ok(await _service.GetAllAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SupplyPlanningResponse>> GetById(
        int id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        return result is null ? NotFound(new { message = $"Supply plan {id} was not found." }) : Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<SupplyPlanningResponse>> Update(
        int id, [FromBody] SupplyPlanningRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(id, request, cancellationToken);
        return result is null ? NotFound(new { message = $"Supply plan {id} was not found." }) : Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _service.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound(new { message = $"Supply plan {id} was not found." });
    }
}
