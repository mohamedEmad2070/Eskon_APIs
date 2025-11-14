using Eskon_APIs.Contracts.House;

namespace Eskon_APIs.Services;

public interface IHouseService
{
    Task<List<HouseSummaryResponse>> GetAllAsync(string? CurrentUserId, CancellationToken cancellationToken = default);
}
