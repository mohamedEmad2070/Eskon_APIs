using Eskon_APIs.Contracts.House;
using Eskon_APIs.Contracts.MediaItem;
using System.Security.Claims;

namespace Eskon_APIs.Controllers;

/// <summary>
/// Manages house-related operations including viewing, creating, updating, and deleting houses.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class HouseController : ControllerBase
{
    private readonly IHouseService _houseService;
    private readonly IMediaService _mediaService;

    /// <summary>
    /// Initializes a new instance of the HouseController.
    /// </summary>
    /// <param name="houseService">The house service for handling house operations.</param>
    public HouseController(IHouseService houseService, IMediaService mediaService)
    {
        _houseService = houseService;
        _mediaService = mediaService;
    }

    /// <summary>
    /// Retrieves all available houses.
    /// </summary>
    /// <remarks>
    /// This endpoint returns a list of all houses with their summary information.
    /// - Authenticated users will see which houses they have saved/favorited.
    /// - Anonymous users will see all houses without any saved status.
    /// </remarks>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>A list of house summary responses.</returns>
    /// <response code="200">Returns the list of all houses.</response>
    [HttpGet("")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(List<HouseSummaryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllHouses(CancellationToken cancellationToken)
    {
        var CurrentUserId = User.Identity?.IsAuthenticated == true
            ? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            : null;

        var houses = await _houseService.GetAllAsync(CurrentUserId, cancellationToken);

        return Ok(houses);
    }

    /// <summary>
    /// Retrieves all house listings owned by the current authenticated user.
    /// </summary>
    /// <remarks>
    /// This endpoint returns a list of all houses that belong to the authenticated user.
    /// Only the owner of a house or an administrator can view their own listings.
    /// 
    /// Use this endpoint to:
    /// - View all properties you have listed
    /// - Manage your property inventory
    /// - Monitor your active listings
    /// 
    /// Requirements:
    /// - User must be authenticated with role 'Member' or 'Admin'
    /// - User will only see their own listings
    /// </remarks>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>A list of house summary responses owned by the current user.</returns>
    /// <response code="200">Returns the list of user's house listings.</response>
    /// <response code="401">Unauthorized - user is not authenticated.</response>
    /// <response code="403">Forbidden - user does not have Member or Admin role.</response>
    [HttpGet("my-listings")]
    [Authorize(Roles = "Member, Admin")]
    [ProducesResponseType(typeof(List<HouseSummaryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetMyListings(CancellationToken cancellationToken)
    {
        var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(ownerId))
        {
            return Unauthorized();
        }

        var result = await _houseService.GetMyListingsAsync(ownerId, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Creates a new house listing.
    /// </summary>
    /// <remarks>
    /// This endpoint allows authenticated members and administrators to create a new house listing.
    /// The user making the request will be set as the owner of the house.
    /// 
    /// Requirements:
    /// - User must be authenticated with role 'Member' or 'Admin'
    /// - Title and Description are required
    /// - Number of rooms must be between 0 and 50
    /// - Number of bathrooms must be between 1 and 20
    /// - Area must be between 1 and 10000 square units
    /// </remarks>
    /// <param name="request">The house creation request containing house details.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>The newly created house detail with generated ID.</returns>
    /// <response code="201">House created successfully. Returns the created house details with the assigned ID.</response>
    /// <response code="400">Bad request - validation failed (e.g., invalid input data).</response>
    /// <response code="401">Unauthorized - user is not authenticated or has insufficient permissions.</response>
    [HttpPost("")]
    [Authorize(Roles = "Member, Admin")]
    [ProducesResponseType(typeof(HouseDetailResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create(CreateHouseRequest request, CancellationToken cancellationToken)
    {
        var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(ownerId))
        {
            return Unauthorized();
        }

        var result = await _houseService.CreateAsync(request, ownerId, cancellationToken);

        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(Get), new { id = result.Value.HouseId }, result.Value);
        }

        return Problem(statusCode: result.Error.StatusCode, title: result.Error.Code, detail: result.Error.Description);
    }

    /// <summary>
    /// Retrieves detailed information about a specific house.
    /// </summary>
    /// <remarks>
    /// This endpoint returns complete details of a house including:
    /// - House information (title, description, price, rooms, bathrooms, area)
    /// - Owner information
    /// - Location details
    /// - All amenities
    /// - Image URLs
    /// - Whether the current user has saved this house (if authenticated)
    /// 
    /// This endpoint is accessible to both authenticated and anonymous users.
    /// </remarks>
    /// <param name="id">The unique identifier of the house.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>Detailed house information.</returns>
    /// <response code="200">Returns the house details.</response>
    /// <response code="404">House not found.</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(HouseDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _houseService.GetAsync(id, currentUserId, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : Problem(statusCode: result.Error.StatusCode, title: result.Error.Code, detail: result.Error.Description);
    }

    /// <summary>
    /// Updates an existing house listing.
    /// </summary>
    /// <remarks>
    /// This endpoint allows the house owner or administrators to update house details.
    /// The user making the request must be the owner of the house or an administrator.
    /// 
    /// Updatable fields:
    /// - Title
    /// - Description
    /// - Price per month
    /// - Number of rooms (0-50)
    /// - Number of bathrooms (1-20)
    /// - Area (1-10000 square units)
    /// - Location
    /// - Amenities
    /// </remarks>
    /// <param name="id">The unique identifier of the house to update.</param>
    /// <param name="request">The house update request containing updated details.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">House updated successfully.</response>
    /// <response code="400">Bad request - validation failed.</response>
    /// <response code="401">Unauthorized - user is not authenticated or is not the house owner.</response>
    /// <response code="404">House not found.</response>
    [HttpPut("{id}")]
    [Authorize(Roles = "Member, Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, UpdateHouseRequest request, CancellationToken cancellationToken)
    {
        var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(ownerId))
        {
            return Unauthorized();
        }

        var result = await _houseService.UpdateAsync(id, request, ownerId, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : Problem(statusCode: result.Error.StatusCode, title: result.Error.Code, detail: result.Error.Description);
    }

    /// <summary>
    /// Deletes a house listing.
    /// </summary>
    /// <remarks>
    /// This endpoint allows the house owner or administrators to delete a house listing.
    /// The user making the request must be the owner of the house or an administrator.
    /// 
    /// Deletion:
    /// - Removes the house from the system
    /// - Removes all associated amenities
    /// - Removes from all users' saved/favorite lists
    /// - Deletes all associated images
    /// </remarks>
    /// <param name="id">The unique identifier of the house to delete.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">House deleted successfully.</response>
    /// <response code="401">Unauthorized - user is not authenticated or is not the house owner.</response>
    /// <response code="404">House not found.</response>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Member, Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(ownerId))
        {
            return Unauthorized();
        }

        var result = await _houseService.DeleteAsync(id, ownerId, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : Problem(statusCode: result.Error.StatusCode, title: result.Error.Code, detail: result.Error.Description);
    }

    /// <summary>
    /// Uploads an image for a house listing.
    /// </summary>
    /// <remarks>
    /// This endpoint allows the house owner or administrators to upload images for a house listing.
    /// 
    /// Requirements:
    /// - User must be authenticated with role 'Member' or 'Admin'
    /// - User must be the owner of the house or an administrator
    /// - The house must exist
    /// - File must be a valid image (JPEG, PNG, WebP, etc.)
    /// - File size should be within acceptable limits
    /// 
    /// Features:
    /// - Supports multiple image uploads per house
    /// - First uploaded image becomes the cover image by default
    /// - Images are stored securely with unique identifiers
    /// - Returns media item details including URL and sort order
    /// 
    /// The uploaded image will be assigned a unique media ID and can be:
    /// - Set as the cover image using the set-cover endpoint
    /// - Deleted using the delete endpoint
    /// - Reordered within the gallery
    /// </remarks>
    /// <param name="houseId">The unique identifier of the house to upload an image for.</param>
    /// <param name="file">The image file to upload. Must be sent as multipart/form-data.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>The uploaded media item details including the image URL.</returns>
    /// <response code="200">Image uploaded successfully. Returns the media item details.</response>
    /// <response code="400">Bad request - invalid file or missing required data.</response>
    /// <response code="401">Unauthorized - user is not authenticated.</response>
    /// <response code="403">Forbidden - user is not the house owner or admin.</response>
    /// <response code="404">House not found.</response>
    /// <response code="413">Payload too large - file exceeds size limit.</response>
    [HttpPost("{houseId}/images")]
    [Authorize(Roles = "Member, Admin")]
    [ProducesResponseType(typeof(MediaItemResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status413PayloadTooLarge)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadImage(int houseId, [FromForm] UploadImageRequest request, CancellationToken cancellationToken)
    {
        var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(ownerId))
        {
            return Unauthorized();
        }

        var result = await _mediaService.UploadImageForHouseAsync(houseId, ownerId, request.File, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : Problem(statusCode: result.Error.StatusCode, title: result.Error.Code, detail: result.Error.Description);
    }

    /// <summary>
    /// Deletes an image from a house listing.
    /// </summary>
    /// <remarks>
    /// This endpoint allows the house owner or administrators to delete an image from a house listing.
    /// 
    /// Requirements:
    /// - User must be authenticated with role 'Member' or 'Admin'
    /// - User must be the owner of the house or an administrator
    /// - The house must exist
    /// - The media item must exist and belong to the specified house
    /// 
    /// Behavior:
    /// - Deletes the image file from storage
    /// - Removes the media item record from the database
    /// - If the deleted image was the cover image, the system will assign a new cover image if available
    /// - Returns no content on successful deletion
    /// </remarks>
    /// <param name="houseId">The unique identifier of the house containing the image.</param>
    /// <param name="mediaItemId">The unique identifier of the media item to delete.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">Image deleted successfully.</response>
    /// <response code="401">Unauthorized - user is not authenticated.</response>
    /// <response code="403">Forbidden - user is not the house owner or admin.</response>
    /// <response code="404">House or media item not found.</response>
    [HttpDelete("{houseId}/images/{mediaItemId}")]
    [Authorize(Roles = "Member, Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteImage(int houseId, int mediaItemId, CancellationToken cancellationToken)
    {
        var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(ownerId))
        {
            return Unauthorized();
        }

        var result = await _mediaService.DeleteImageAsync(houseId, ownerId, mediaItemId, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : Problem(statusCode: result.Error.StatusCode, title: result.Error.Code, detail: result.Error.Description);
    }

    /// <summary>
    /// Sets an image as the cover image for a house listing.
    /// </summary>
    /// <remarks>
    /// This endpoint allows the house owner or administrators to set a specific image as the cover/thumbnail image for a house listing.
    /// 
    /// Requirements:
    /// - User must be authenticated with role 'Member' or 'Admin'
    /// - User must be the owner of the house or an administrator
    /// - The house must exist
    /// - The media item must exist and belong to the specified house
    /// 
    /// Behavior:
    /// - Sets the IsCover flag to true for the specified media item
    /// - Automatically sets IsCover to false for any previously set cover image
    /// - Only one image can be the cover image at a time
    /// - The cover image is displayed as the thumbnail/main image for the house in listings
    /// 
    /// Best Practices:
    /// - Set a high-quality, well-lit image as the cover
    /// - Ensure the cover image effectively represents the property
    /// - Update the cover image when you change the main presentation of the property
    /// </remarks>
    /// <param name="houseId">The unique identifier of the house.</param>
    /// <param name="mediaItemId">The unique identifier of the media item to set as cover.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">Cover image set successfully.</response>
    /// <response code="401">Unauthorized - user is not authenticated.</response>
    /// <response code="403">Forbidden - user is not the house owner or admin.</response>
    /// <response code="404">House or media item not found.</response>
    [HttpPut("{houseId}/images/{mediaItemId}/set-cover")]
    [Authorize(Roles = "Member, Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetCoverImage(int houseId, int mediaItemId, CancellationToken cancellationToken)
    {
        var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(ownerId))
        {
            return Unauthorized();
        }

        var result = await _mediaService.SetCoverImageAsync(houseId, ownerId, mediaItemId, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : Problem(statusCode: result.Error.StatusCode, title: result.Error.Code, detail: result.Error.Description);
    }
}
