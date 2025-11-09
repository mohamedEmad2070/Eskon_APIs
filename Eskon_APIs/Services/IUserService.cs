using Eskon_APIs.Contracts.Users;
using Eskon_APIs.Abstraction;

namespace Eskon_APIs.Services;

public interface IUserService
{
    Task<Result<UserProfileResponse>> GetProfileAsync(string userId, CancellationToken cancellationToken = default);

}
