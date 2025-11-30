using Eskon_APIs.Contracts.Amenity;
using Eskon_APIs.Contracts.Location;
using Eskon_APIs.Contracts.MediaItem;

namespace Eskon_APIs.Contracts.House;

public class HouseDetailResponse
{
    public int HouseId { get; set; }

    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal PricePerMonth { get; set; }
    public int NumberOfRooms { get; set; }
    public int NumberOfBathrooms { get; set; }
    public double Area { get; set; }

    public HouseOwnerResponse Owner { get; set; }

    public LocationResponse Location { get; set; }

    public List<MediaItemResponse> MediaItems { get; set; } = new();

    public List<AmenityResponse> Amenities { get; set; } = new();

    public bool isSavedByCurrrentUser { get; set; }
}
