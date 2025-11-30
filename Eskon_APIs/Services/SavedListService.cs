using Eskon_APIs.Errors;

namespace Eskon_APIs.Services;

public class SavedListService(ApplicationDbContext dbContext) : ISavedListService
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Result> SaveAsync(string userId, int houseId, CancellationToken cancellationToken = default)
    {
        // Check if house exists
        var houseExists = await _dbContext.House
            .AnyAsync(h => h.HouseId == houseId, cancellationToken);

        if (!houseExists)
            return Result.Failure(SavedListErrors.HouseNotFound);

        // Check if already saved
        var alreadySaved = await _dbContext.SavedList
            .AnyAsync(s => s.UserId == userId && s.HouseId == houseId, cancellationToken);

        if (alreadySaved)
            return Result.Failure(SavedListErrors.AlreadyExists);

        var saved = new SavedList
        {
            UserId = userId,
            HouseId = houseId,
            SavedAt = DateTime.UtcNow
        };

        _dbContext.SavedList.Add(saved);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> UnsaveAsync(string userId, int houseId, CancellationToken cancellationToken = default)
    {
        var saved = await _dbContext.SavedList
            .FirstOrDefaultAsync(s => s.UserId == userId && s.HouseId == houseId, cancellationToken);

        if (saved is null)
            return Result.Failure(SavedListErrors.NotFound);

        _dbContext.SavedList.Remove(saved);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<bool>> IsSavedAsync(string userId, int houseId, CancellationToken cancellationToken = default)
    {
        var exists = await _dbContext.SavedList
            .AnyAsync(s => s.UserId == userId && s.HouseId == houseId, cancellationToken);

        return Result<bool>.Success(exists);
    }

    public async Task<Result<List<House>>> GetSavedHousesAsync(string userId, CancellationToken cancellationToken = default)
    {
        var houses = await _dbContext.SavedList
            .Where(s => s.UserId == userId)
            .Select(s => s.House)
            .ToListAsync(cancellationToken);

        return Result<List<House>>.Success(houses);
    }
}
