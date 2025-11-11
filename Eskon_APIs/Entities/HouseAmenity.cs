namespace Eskon_APIs.Entities
{
    public class HouseAmenity
    {
        public int HouseId { get; set; }
        public int AmenityId { get; set; }

        public Amenity Amenity { get; set; } = null!;
        public House House { get; set; } = null!;
    }
}
