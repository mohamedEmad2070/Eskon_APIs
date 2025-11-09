using Eskon_APIs.Contracts.Users;
using Eskon_APIs.Entities;
using Eskon_APIs.Errors;
using Eskon_APIs.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Eskon_APIs.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<UserProfileResponse>> GetProfileAsync(string userId, CancellationToken cancellationToken = default)
    {
        
        var profile = await _userManager.Users
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .Select(u => new UserProfileResponse(
                u.Email!,
                u.UserName ?? string.Empty,
                u.FirstName,
                u.LastName
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (profile is null)
            return Result.Failure<UserProfileResponse>(UserErrors.InvalidCredentials);

        return Result.Success(profile);
    }


}
