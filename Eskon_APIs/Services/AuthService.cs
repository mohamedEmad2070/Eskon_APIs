
using Eskon_APIs.Authentication;

namespace Eskon_APIs.Services;

public class AuthService(UserManager<ApplicationUser> userManager,IJwtProvider jwtProvider) : IAuthService
{
    private readonly UserManager<ApplicationUser> _UserManager = userManager;
    private readonly IJwtProvider _JwtProvider = jwtProvider;

    public Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
