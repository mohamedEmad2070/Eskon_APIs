using Eskon_APIs.Contracts.Location;

namespace Eskon_APIs.Controllers;

/// <summary>
/// Manages location-related operations including viewing, creating, updating, and deleting locations.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class LocationsController : ControllerBase
{
    private readonly ILocationService _locationService;

    /// <summary>
    /// Initializes a new instance of the LocationsController.
    /// </summary>
    /// <param name="locationService">The location service for handling location operations.</param>
    public LocationsController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    /// <summary>
    /// Retrieves all available locations.
    /// </summary>
    /// <remarks>
    /// This endpoint returns a list of all locations in the system with their complete information.
    /// This includes:
    /// - Country and City
    /// - Street address and Building number
    /// - Postal code (if available)
    /// - Geographic coordinates (latitude and longitude, if available)
    /// 
    /// Use this endpoint to:
    /// - Get a complete list of all available locations
    /// - Populate dropdown menus or location selectors
    /// - Browse all locations in the system
    /// 
    /// No authentication is required to view locations.
    /// </remarks>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>A list of all available locations.</returns>
    /// <response code="200">Returns the list of all locations.</response>
    [HttpGet("")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<LocationResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var locations = await _locationService.GetAllAsync(cancellationToken);
        return Ok(locations);
    }

    /// <summary>
    /// Retrieves a specific location by its ID.
    /// </summary>
    /// <remarks>
    /// This endpoint returns detailed information about a specific location including:
    /// - Country and City
    /// - Street address and Building number
    /// - Postal code (if available)
    /// - Geographic coordinates (latitude and longitude, if available)
    /// 
    /// No authentication is required to view location details.
    /// </remarks>
    /// <param name="id">The unique identifier of the location.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>The location details.</returns>
    /// <response code="200">Returns the location details.</response>
    /// <response code="404">Location not found.</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LocationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var locationResult = await _locationService.GetAsync(id, cancellationToken);

        return locationResult.IsSuccess ? Ok(locationResult.Value)
            : Problem(statusCode: StatusCodes.Status404NotFound, title: locationResult.Error.Code, detail: locationResult.Error.Description);
    }

    /// <summary>
    /// Creates a new location.
    /// </summary>
    /// <remarks>
    /// This endpoint allows authenticated members and administrators to add a new location to the system.
    /// 
    /// Requirements:
    /// - User must be authenticated with role 'Member' or 'Admin'
    /// - Country, City, Street, and BuildingNumber are required
    /// - PostalCode is optional
    /// - Geographic coordinates (GeoLat, GeoLng) are optional
    /// 
    /// Common location format: "Street Number, Street Name, Postal Code, City, Country"
    /// </remarks>
    /// <param name="locationRequest">The location creation request containing location details.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>The newly created location with the generated ID.</returns>
    /// <response code="201">Location created successfully. Returns the created location details with the assigned ID.</response>
    /// <response code="400">Bad request - validation failed (e.g., missing required fields or duplicate location).</response>
    /// <response code="401">Unauthorized - user is not authenticated.</response>
    /// <response code="403">Forbidden - user does not have Member or Admin role.</response>
    [HttpPost("")]
    [Authorize(Roles = "Admin, Member")]
    [ProducesResponseType(typeof(LocationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Add([FromBody] LocationRequest locationRequest, CancellationToken cancellationToken)
    {
        var addedLocationResult = await _locationService.AddAsync(locationRequest, cancellationToken);

        return addedLocationResult.IsSuccess
            ? CreatedAtAction(nameof(Get), new { id = addedLocationResult.Value.LocationId }, addedLocationResult.Value)
            : Problem(statusCode: StatusCodes.Status400BadRequest, title: addedLocationResult.Error.Code, detail: addedLocationResult.Error.Description);
    }

    /// <summary>
    /// Updates an existing location.
    /// </summary>
    /// <remarks>
    /// This endpoint allows administrators to update location details.
    /// 
    /// Requirements:
    /// - User must be authenticated with role 'Admin'
    /// - The location must exist
    /// 
    /// Updatable fields:
    /// - Country
    /// - City
    /// - Street
    /// - BuildingNumber
    /// - PostalCode
    /// - GeoLat (latitude)
    /// - GeoLng (longitude)
    /// </remarks>
    /// <param name="id">The unique identifier of the location to update.</param>
    /// <param name="locationRequest">The location update request containing updated details.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">Location updated successfully.</response>
    /// <response code="400">Bad request - validation failed.</response>
    /// <response code="401">Unauthorized - user is not authenticated.</response>
    /// <response code="403">Forbidden - user does not have Admin role.</response>
    /// <response code="404">Location not found.</response>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, LocationRequest locationRequest, CancellationToken cancellationToken)
    {
        var updateResult = await _locationService.UpdateAsync(id, locationRequest, cancellationToken);

        return updateResult.IsSuccess ? NoContent()
           : Problem(statusCode: StatusCodes.Status404NotFound, title: updateResult.Error.Code, detail: updateResult.Error.Description);
    }

    /// <summary>
    /// Deletes a location.
    /// </summary>
    /// <remarks>
    /// This endpoint allows administrators to delete a location from the system.
    /// 
    /// Requirements:
    /// - User must be authenticated with role 'Admin'
    /// - The location must exist
    /// 
    /// Note: Deleting a location may affect houses that reference this location.
    /// Consider updating those houses first if needed.
    /// </remarks>
    /// <param name="id">The unique identifier of the location to delete.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">Location deleted successfully.</response>
    /// <response code="401">Unauthorized - user is not authenticated.</response>
    /// <response code="403">Forbidden - user does not have Admin role.</response>
    /// <response code="404">Location not found.</response>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var deleteResult = await _locationService.DeleteAsync(id, cancellationToken);
        return deleteResult.IsSuccess ? NoContent() :
            Problem(statusCode: StatusCodes.Status404NotFound, title: deleteResult.Error.Code, detail: deleteResult.Error.Description);
    }
}
