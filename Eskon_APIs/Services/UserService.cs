using Eskon_APIs.Contracts.Users;
using Eskon_APIs.Errors;

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

    public async Task<Result<UserProfileResponse>> GetProfileAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
            return Result.Failure<UserProfileResponse>(UserErrors.InvalidCredentials);

        // Check if user is in the Admin role
        bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

        var profile = new UserProfileResponse(
            user.Email!,
            user.UserName ?? string.Empty,
            user.FirstName,
            user.LastName,
            isAdmin
        );

        return Result.Success(profile);
    }


    public async Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId);

        var result = await _userManager.ChangePasswordAsync(user!, request.CurrentPassword, request.NewPassword);

        if (result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }


}
