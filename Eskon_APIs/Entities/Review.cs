using System.ComponentModel.DataAnnotations;

namespace Eskon_APIs.Entities;

public class Review
{
    public int ReviewId { get; set; }

    [Range(1, 5)]
    public int Stars { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int HouseId { get; set; }
    public House House { get; set; } = null!;

    public string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
}
