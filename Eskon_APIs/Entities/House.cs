namespace Eskon_APIs.Entities
{
    public class House
    {
        public int HouseId { get; set; }

        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal PricePerMonth { get; set; }
        public int NumberOfRooms { get; set; }
        public int NumberOfBathrooms { get; set; }
        public double Area { get; set; }
        public double? AverageRating { get; set; }
        public int RatingCount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public string OwnerId { get; set; } = null!;
        public ApplicationUser Owner { get; set; } = null!;


        [ForeignKey("Location")]
        public int LocationId { get; set; }
        public Location Location { get; set; } = null!;

        public ICollection<MediaItem> MediaItems { get; set; } = new List<MediaItem>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<SavedList> SavedBy { get; set; } = new List<SavedList>();
        public ICollection<HouseAmenity> HouseAmenities { get; set; } = new List<HouseAmenity>();
    }
}
