using Eskon_APIs.Contracts.Users;
using Eskon_APIs.Extensions;

namespace Eskon_APIs.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class AccountController(IUserService userService, ILogger<AccountController> logger) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly ILogger<AccountController> _logger = logger;

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        var result = await _userService.GetProfileAsync(User.GetUserId()!,cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var result = await _userService.ChangePasswordAsync(User.GetUserId()!, request);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}