using Eskon_APIs.Contracts.Users;
using Eskon_APIs.Extensions;

namespace Eskon_APIs.Controllers;

/// <summary>
/// Account controller for managing user profile and account settings.
/// Provides endpoints to retrieve and update user profile information.
/// All endpoints require authentication via JWT Bearer token.
/// </summary>
[Route("[controller]")]
[ApiController]
[Authorize]
public class AccountController(IUserService userService, ILogger<AccountController> logger) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly ILogger<AccountController> _logger = logger;

    /// <summary>
    /// Retrieves the current authenticated user's profile information.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /account/profile
    ///
    /// This endpoint requires a valid JWT Bearer token in the Authorization header.
    /// </remarks>
    /// <param name="cancellationToken">A cancellation token to cancel the operation</param>
    /// <returns>The user's profile information including email, first name, and last name</returns>
    /// <response code="200">Successfully retrieved the user profile</response>
    /// <response code="401">Unauthorized - JWT token is missing or invalid</response>
    /// <response code="500">Internal server error while retrieving the profile</response>
    [HttpGet("profile")]
    [Produces("application/json")]
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        var result = await _userService.GetProfileAsync(User.GetUserId()!,cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    /// <summary>
    /// Changes the password for the current authenticated user.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /account/change-password
    ///     {
    ///       "currentPassword": "CurrentPassword123!",
    ///       "newPassword": "NewPassword123!",
    ///       "confirmPassword": "NewPassword123!"
    ///     }
    ///
    /// This endpoint requires a valid JWT Bearer token in the Authorization header.
    /// </remarks>
    /// <param name="request">The change password request containing current and new password</param>
    /// <returns>No content on success</returns>
    /// <response code="204">Password changed successfully</response>
    /// <response code="400">Bad request - invalid password format or validation failure</response>
    /// <response code="401">Unauthorized - JWT token is missing or invalid</response>
    /// <response code="500">Internal server error while changing the password</response>
    [HttpPut("change-password")]
    [Consumes("application/json")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var result = await _userService.ChangePasswordAsync(User.GetUserId()!, request);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}