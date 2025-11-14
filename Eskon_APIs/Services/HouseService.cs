using Eskon_APIs.Contracts.House;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Eskon_APIs.Services;

public class HouseService : IHouseService
{
    private readonly ApplicationDbContext _context;

    public HouseService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<HouseSummaryResponse>> GetAllAsync(string? CurrentUserId, CancellationToken cancellationToken = default)
    {
        var houses = await _context.House
            .AsNoTracking()
            .Include(h => h.MediaItems)
            .Include(h => h.Location)
            .ToListAsync(cancellationToken);

       
        var savedHousesIds = CurrentUserId != null
            ? await _context.SavedList
            .Where(SavedList => SavedList.UserId == CurrentUserId)
            .Select(SavedList => SavedList.HouseId)
            .ToListAsync(cancellationToken) : new List<int>();
        

        var houseSummaryResponses = houses.Adapt<List<HouseSummaryResponse>>();

        foreach (var house in houseSummaryResponses)
        {
            house.IsSavedByCurrentUser = savedHousesIds.Contains(house.HouseId);
        }

        return houseSummaryResponses;
    }
}