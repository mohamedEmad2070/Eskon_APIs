using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        var houses = await _houseService.GetAllAsync(cancellationToken);
        return Ok(houses);
    }
}
