using Eskon_APIs.Errors;
using Eskon_APIs.Extensions;

namespace Eskon_APIs.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Member,Admin")]
public class SavedListController : ControllerBase
{
    private readonly ISavedListService _savedListService;

    public SavedListController(ISavedListService savedListService)
    {
        _savedListService = savedListService;
    }

    /// <summary>
    /// Adds a house to the user's saved list.
    /// </summary>
    /// <param name="houseId">The ID of the house to save.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Success message.</returns>
    /// <response code="200">House successfully saved.</response>
    /// <response code="404">User or house not found.</response>
    /// <response code="409">House already exists in saved list.</response>
    [HttpPost("{houseId:int}")]
    public async Task<IActionResult> Save(int houseId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        if (userId is null)
            return Problem(SavedListErrors.UserNotFound);

        var result = await _savedListService.SaveAsync(userId, houseId, cancellationToken);

        if (result.IsFailure)
            return Problem(result.Error);

        return Ok(new { message = "House added to saved list." });
    }

    /// <summary>
    /// Removes a saved house from the user's saved list.
    /// </summary>
    /// <param name="houseId">The ID of the house to remove.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Success message.</returns>
    /// <response code="200">House successfully removed.</response>
    /// <response code="404">User or saved item not found.</response>
    [HttpDelete("{houseId:int}")]
    public async Task<IActionResult> Unsave(int houseId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        if (userId is null)
            return Problem(SavedListErrors.UserNotFound);

        var result = await _savedListService.UnsaveAsync(userId, houseId, cancellationToken);

        if (result.IsFailure)
            return Problem(result.Error);

        return Ok(new { message = "House removed from saved list." });
    }

    /// <summary>
    /// Checks if a specific house is saved by the user.
    /// </summary>
    /// <param name="houseId">The ID of the house.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Boolean indicating if saved.</returns>
    /// <response code="200">Returns whether the house is saved.</response>
    /// <response code="404">User not found.</response>
    [HttpGet("{houseId:int}")]
    public async Task<IActionResult> IsSaved(int houseId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        if (userId is null)
            return Problem(SavedListErrors.UserNotFound);

        var result = await _savedListService.IsSavedAsync(userId, houseId, cancellationToken);

        if (result.IsFailure)
            return Problem(result.Error);

        return Ok(new { isSaved = result.Value });
    }

    /// <summary>
    /// Gets all saved houses for the current user.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of saved houses.</returns>
    /// <response code="200">Returns the saved list.</response>
    /// <response code="404">User not found.</response>
    [HttpGet]
    public async Task<IActionResult> GetSavedList(CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        if (userId is null)
            return Problem(SavedListErrors.UserNotFound);

        var result = await _savedListService.GetSavedHousesAsync(userId, cancellationToken);

        if (result.IsFailure)
            return Problem(result.Error);

        return Ok(result.Value);
    }

    private ObjectResult Problem(Error error)
    {
        return Problem(
            title: error.Code,
            detail: error.Description,
            statusCode: error.StatusCode
        );
    }
}
