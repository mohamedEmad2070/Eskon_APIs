namespace Eskon_APIs.Entities
{
    public class Amenity
    {
        public int AmenityId { get; set; }

        public string AmenityName { get; set; } = null!;
        public string? Category { get; set; }

        public ICollection<HouseAmenity> HouseAmenities { get; set; } = new List<HouseAmenity>();
    }
}
