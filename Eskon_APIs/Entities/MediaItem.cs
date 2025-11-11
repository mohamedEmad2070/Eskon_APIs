using System.ComponentModel.DataAnnotations.Schema;

namespace Eskon_APIs.Entities;

public class MediaItem
{
    public int MediaId { get; set; }
    public string URL { get; set; } = null!;
    public int SortOrder { get; set; }
    public bool IsCover { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    
    public int HouseId { get; set; }
    public House House { get; set; } = null!;
}
