using Eskon_APIs.Contracts.Amenity;

namespace Eskon_APIs.Controllers;

/// <summary>
/// Manages amenities operations including viewing, creating, updating, and deleting amenities.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class AmenitiesController : ControllerBase
{
    private readonly IAmenityService _amenityService;

    /// <summary>
    /// Initializes a new instance of the AmenitiesController.
    /// </summary>
    /// <param name="amenityService">The amenity service for handling amenity operations.</param>
    public AmenitiesController(IAmenityService amenityService)
    {
        _amenityService = amenityService;
    }

    /// <summary>
    /// Retrieves all available amenities.
    /// </summary>
    /// <remarks>
    /// This endpoint returns a list of all amenities in the system.
    /// No authentication is required to view amenities.
    /// </remarks>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>A list of all amenities.</returns>
    /// <response code="200">Returns the list of all amenities.</response>
    [HttpGet("")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<AmenityResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var amenities = await _amenityService.GetAllAsync(cancellationToken);

        return Ok(amenities);
    }

    /// <summary>
    /// Retrieves a specific amenity by its ID.
    /// </summary>
    /// <remarks>
    /// This endpoint returns detailed information about a specific amenity.
    /// No authentication is required to view amenity details.
    /// </remarks>
    /// <param name="id">The unique identifier of the amenity.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>The amenity details.</returns>
    /// <response code="200">Returns the amenity details.</response>
    /// <response code="404">Amenity not found.</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AmenityResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var amenityResult = await _amenityService.GetAsync(id, cancellationToken);

        return amenityResult.IsSuccess ? Ok(amenityResult.Value) :
   Problem(statusCode: StatusCodes.Status404NotFound, title: amenityResult.Error.Code, detail: amenityResult.Error.Description);
    }

    /// <summary>
    /// Creates a new amenity.
    /// </summary>
    /// <remarks>
    /// This endpoint allows administrators to add a new amenity to the system.
    /// 
    /// Requirements:
    /// - User must be authenticated with role 'Admin'
    /// - AmenityName is required
    /// - Category is optional
    /// </remarks>
    /// <param name="amenityRequest">The amenity creation request containing amenity details.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>The newly created amenity.</returns>
    /// <response code="201">Amenity created successfully. Returns the created amenity with the assigned ID.</response>
    /// <response code="400">Bad request - validation failed (e.g., missing required fields).</response>
    /// <response code="401">Unauthorized - user is not authenticated.</response>
    /// <response code="403">Forbidden - user does not have Admin role.</response>
    [HttpPost("")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(AmenityResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Add([FromBody] AmenityRequest amenityRequest, CancellationToken cancellationToken = default)
    {
        var addedAmenity = await _amenityService.AddAsync(amenityRequest, cancellationToken);

        return CreatedAtAction(nameof(Get), new { id = addedAmenity.AmenityId }, addedAmenity);
    }

    /// <summary>
    /// Deletes an amenity.
    /// </summary>
    /// <remarks>
    /// This endpoint allows administrators to delete an amenity from the system.
    /// 
    /// Requirements:
    /// - User must be authenticated with role 'Admin'
    /// - The amenity must exist
    /// 
    /// Note: Deleting an amenity may affect houses that have this amenity listed.
    /// </remarks>
    /// <param name="id">The unique identifier of the amenity to delete.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">Amenity deleted successfully.</response>
    /// <response code="401">Unauthorized - user is not authenticated.</response>
    /// <response code="403">Forbidden - user does not have Admin role.</response>
    /// <response code="404">Amenity not found.</response>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var deleteResult = await _amenityService.DeleteAsync(id, cancellationToken);

        return deleteResult.IsSuccess ? NoContent() :
            Problem(statusCode: StatusCodes.Status404NotFound, title: deleteResult.Error.Code, detail: deleteResult.Error.Description);
    }

    /// <summary>
    /// Updates an existing amenity.
    /// </summary>
    /// <remarks>
    /// This endpoint allows administrators to update amenity details.
    /// 
    /// Requirements:
    /// - User must be authenticated with role 'Admin'
    /// - The amenity must exist
    /// 
    /// Updatable fields:
    /// - AmenityName
    /// - Category
    /// </remarks>
    /// <param name="id">The unique identifier of the amenity to update.</param>
    /// <param name="amenityRequest">The amenity update request containing updated details.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">Amenity updated successfully.</response>
    /// <response code="400">Bad request - validation failed.</response>
    /// <response code="401">Unauthorized - user is not authenticated.</response>
    /// <response code="403">Forbidden - user does not have Admin role.</response>
    /// <response code="404">Amenity not found.</response>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] AmenityRequest amenityRequest, CancellationToken cancellationToken = default)
    {
        var updateResult = await _amenityService.UpdateAsync(id, amenityRequest, cancellationToken);

        return updateResult.IsSuccess ? NoContent() :
  Problem(statusCode: StatusCodes.Status404NotFound, title: updateResult.Error.Code, detail: updateResult.Error.Description);
    }
}
