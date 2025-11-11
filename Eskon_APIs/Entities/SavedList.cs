namespace Eskon_APIs.Entities
{
    public class SavedList
    {
        public string UserId { get; set; } = null!;
        public int HouseId { get; set; }

        public DateTime SavedAt { get; set; } = DateTime.UtcNow;

        public ApplicationUser User { get; set; } = null!;
        public House House { get; set; } = null!;

    }
}
