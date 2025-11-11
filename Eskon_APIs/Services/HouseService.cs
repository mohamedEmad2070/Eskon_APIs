using Eskon_APIs.Contracts.House;

namespace Eskon_APIs.Services;

public class HouseService : IHouseService
{
    private readonly ApplicationDbContext _context;

    public HouseService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<HouseSummaryResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var houses = await _context.House
            .AsNoTracking()
            .Include(h => h.MediaItems)
            .Include(h => h.Location)
            .ToListAsync(cancellationToken);


        return houses.Adapt<List<HouseSummaryResponse>>();
    }
}
