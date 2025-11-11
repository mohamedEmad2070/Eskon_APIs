namespace Eskon_APIs.Entities
{
    public class Location
    {
        public int LocationId { get; set; }

        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string? PostalCode { get; set; }
        public string Street { get; set; } = string.Empty;
        public string BuildingNumber { get; set; } = string.Empty;
        public string? GeoLat { get; set; }
        public string? GeoLng { get; set; }

        public ICollection<House> Houses { get; set; } = new List<House>();
    }
}
