namespace Eskon_APIs.Contracts.Amenity;

/// <summary>
/// Response model containing amenity information.
/// </summary>
public class AmenityResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the amenity.
    /// </summary>
    public int AmenityId { get; set; }

    /// <summary>
    /// Gets or sets the name of the amenity.
    /// </summary>
    /// <remarks>
    /// Examples: "WiFi", "Parking", "Swimming Pool", "Air Conditioning", "Garden"
    /// </remarks>
    public string AmenityName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the category of the amenity.
    /// </summary>
    /// <remarks>
    /// Examples: "Connectivity", "Parking", "Recreation", "Climate Control", "Outdoor"
    /// </remarks>
    public string? Category { get; set; }
}
