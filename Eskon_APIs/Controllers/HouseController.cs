using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Eskon_APIs.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HouseController : ControllerBase
{
    private readonly IHouseService _houseService;
    public HouseController(IHouseService houseService)
    {
        _houseService = houseService;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllHouses(CancellationToken cancellationToken)
    {
        var CurrentUserId = User.Identity?.IsAuthenticated == true
            ? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            : null;

        var houses = await _houseService.GetAllAsync(CurrentUserId, cancellationToken);

        return Ok(houses);
    }
}
