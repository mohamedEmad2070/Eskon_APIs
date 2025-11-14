namespace Eskon_APIs.Contracts.House;

public class HouseSummaryResponse
{
    public int HouseId { get; set; }
    public string Title { get; set; }
    public decimal PricePerMonth { get; set; }
    public int NumberOfRooms { get; set; }
    public int NumberOfBathrooms { get; set; }
    public double Area { get; set; }
    public string CoverImageUrl { get; set; }
    public string FormattedLocation { get; set; }
    public bool IsSavedByCurrentUser { get; set; }
}
