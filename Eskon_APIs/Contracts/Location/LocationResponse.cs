namespace Eskon_APIs.Contracts.Location;

/// <summary>
/// Response model containing location information.
/// </summary>
public class LocationResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the location.
    /// </summary>
    public int LocationId { get; set; }

    /// <summary>
    /// Gets or sets the country of the location.
    /// </summary>
    /// <remarks>
    /// The country name where the location is situated.
    /// Examples: "Egypt", "United States", "United Kingdom"
    /// </remarks>
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the city of the location.
    /// </summary>
    /// <remarks>
    /// The city name where the location is situated.
    /// Examples: "Cairo", "New York", "London"
    /// </remarks>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the postal code of the location.
    /// </summary>
    /// <remarks>
    /// The postal/zip code. This field is optional.
    /// Examples: "12345", "SW1A 1AA"
    /// </remarks>
    public string? PostalCode { get; set; }

    /// <summary>
    /// Gets or sets the street name of the location.
    /// </summary>
    /// <remarks>
    /// The street or road name.
    /// Examples: "Main Street", "Oxford Street", "Nile Avenue"
    /// </remarks>
    public string Street { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the building number of the location.
    /// </summary>
    /// <remarks>
    /// The building or house number on the street.
    /// Examples: "123", "Building A", "Tower 5"
    /// </remarks>
    public string BuildingNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the geographic latitude of the location.
    /// </summary>
    /// <remarks>
    /// The latitude coordinate in decimal format.
    /// Examples: "30.0444", "-73.9857"
    /// This field is optional if not provided during creation.
    /// </remarks>
    public string? GeoLat { get; set; }

    /// <summary>
    /// Gets or sets the geographic longitude of the location.
    /// </summary>
    /// <remarks>
    /// The longitude coordinate in decimal format.
    /// Examples: "31.2357", "-122.4194"
    /// This field is optional if not provided during creation.
    /// </remarks>
    public string? GeoLng { get; set; }
}
