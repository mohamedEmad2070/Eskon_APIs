using Eskon_APIs.Contracts.Location;
using Eskon_APIs.Errors;

namespace Eskon_APIs.Services;

public class LocationService : ILocationService
{
    private readonly ApplicationDbContext _context;

    public LocationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<LocationResponse>> AddAsync(LocationRequest locationRequest, CancellationToken cancellationToken = default)
    {
        var location = locationRequest.Adapt<Location>();

        await _context.Location.AddAsync(location, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(location.Adapt<LocationResponse>());
    }

    public async Task<IEnumerable<LocationResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var locations = await _context.Location.ToListAsync(cancellationToken);
        return locations.Adapt<IEnumerable<LocationResponse>>();
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var location = await _context.Location.FindAsync(id, cancellationToken);

        if (location is null)
        {
            return Result.Failure(LocationErrors.LocationNotFound);
        }

        _context.Remove(location);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<LocationResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var location = await _context.Location.FindAsync(id, cancellationToken);

        return location is not null
            ? Result.Success(location.Adapt<LocationResponse>())
            : Result.Failure<LocationResponse>(LocationErrors.LocationNotFound);
    }

    public async Task<Result> UpdateAsync(int id, LocationRequest locationRequest, CancellationToken cancellationToken = default)
    {
        var currentLocation = await _context.Location.FindAsync(id, cancellationToken);

        if (currentLocation is null)
        {
            return Result.Failure(LocationErrors.LocationNotFound);
        }

        locationRequest.Adapt(currentLocation);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
