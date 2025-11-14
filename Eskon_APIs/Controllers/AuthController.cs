namespace Eskon_APIs.Controllers;

/// <summary>
/// Authentication controller for managing user authentication operations.
/// Provides endpoints for user login, registration, email confirmation, password reset, and token management.
/// </summary>
[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly ILogger<AuthController> _logger = logger;

    /// <summary>
    /// Authenticates a user and returns JWT access token and refresh token.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /auth
    ///     {
    ///       "email": "user@example.com",
    ///       "password": "SecurePassword123!"
    ///     }
    ///
    /// </remarks>
    /// <param name="request">Login credentials containing email and password</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation</param>
    /// <returns>Authentication response with access token and refresh token</returns>
    /// <response code="200">Successfully authenticated and returned authentication tokens</response>
    /// <response code="400">Bad request - invalid credentials or validation failure</response>
    /// <response code="401">Unauthorized - invalid email or password</response>
    /// <response code="500">Internal server error during authentication</response>
    [HttpPost("")]
    [Produces("application/json")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Logging with email: {email} and password: {password}", request.Email, request.Password);

        var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

        return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem();

    }

    /// <summary>
    /// Refreshes the authentication tokens using a valid refresh token.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /auth/refresh
    ///     {
    ///       "token": "eyJhbGciOiJIUzI1NiIs...",
    ///       "refreshToken": "refresh_token_value..."
    ///     }
    ///
    /// </remarks>
    /// <param name="request">Refresh token request containing current token and refresh token</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation</param>
    /// <returns>New authentication response with refreshed tokens</returns>
    /// <response code="200">Successfully refreshed tokens</response>
    /// <response code="400">Bad request - invalid token format or validation failure</response>
    /// <response code="401">Unauthorized - invalid or expired refresh token</response>
    /// <response code="500">Internal server error during token refresh</response>
    [HttpPost("refresh")]
    [Produces("application/json")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem();
    }

    /// <summary>
    /// Revokes a refresh token to invalidate all tokens associated with it.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /auth/revoke-refresh-token
    ///     {
    ///       "token": "eyJhbGciOiJIUzI1NiIs...",
    ///       "refreshToken": "refresh_token_value..."
    ///     }
    ///
    /// </remarks>
    /// <param name="request">Refresh token request containing token and refresh token to revoke</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation</param>
    /// <returns>Success response if token was revoked</returns>
    /// <response code="200">Successfully revoked the refresh token</response>
    /// <response code="400">Bad request - invalid token format or validation failure</response>
    /// <response code="401">Unauthorized - invalid or already revoked token</response>
    /// <response code="500">Internal server error during token revocation</response>
    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    /// <summary>
    /// Registers a new user account.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /auth/register
    ///     {
    ///       "email": "newuser@example.com",
    ///       "password": "SecurePassword123!",
    ///       "confirmPassword": "SecurePassword123!",
    ///       "firstName": "John",
    ///       "lastName": "Doe"
    ///     }
    ///
    /// </remarks>
    /// <param name="request">Registration details including email, password, and personal information</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation</param>
    /// <returns>Success response if registration was successful</returns>
    /// <response code="200">Successfully registered - confirmation email sent</response>
    /// <response code="400">Bad request - invalid input or validation failure</response>
    /// <response code="409">Conflict - email already exists</response>
    /// <response code="500">Internal server error during registration</response>
    [HttpPost("register")]
    [Consumes("application/json")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RegisterAsync(request, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    /// <summary>
    /// Confirms a user's email address using a confirmation token.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /auth/confirm-email
    ///     {
    ///       "email": "user@example.com",
    ///       "token": "confirmation_token_from_email..."
    ///     }
    ///
    /// </remarks>
    /// <param name="request">Email confirmation request containing email and confirmation token</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation</param>
    /// <returns>Success response if email was confirmed</returns>
    /// <response code="200">Successfully confirmed email address</response>
    /// <response code="400">Bad request - invalid token or validation failure</response>
    /// <response code="401">Unauthorized - token expired or invalid</response>
    /// <response code="404">Not found - user email not found</response>
    /// <response code="500">Internal server error during email confirmation</response>
    [HttpPost("confirm-email")]
    [Consumes("application/json")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.ConfirmEmailAsync(request);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    /// <summary>
    /// Resends the email confirmation token to the user's email address.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /auth/resend-confirmation-email
    ///     {
    ///       "email": "user@example.com"
    ///     }
    ///
    /// </remarks>
    /// <param name="request">Resend confirmation request containing the user's email</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation</param>
    /// <returns>Success response if confirmation email was resent</returns>
    /// <response code="200">Successfully resent confirmation email</response>
    /// <response code="400">Bad request - invalid email or validation failure</response>
    /// <response code="404">Not found - user email not found</response>
    /// <response code="500">Internal server error while sending confirmation email</response>
    [HttpPost("resend-confirmation-email")]
    [Consumes("application/json")]
    public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmailRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.ResendConfirmationEmailAsync(request);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    /// <summary>
    /// Initiates password reset by sending a reset code to the user's email.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /auth/forget-password
    ///     {
    ///       "email": "user@example.com"
    ///     }
    ///
    /// </remarks>
    /// <param name="request">Forget password request containing the user's email</param>
    /// <returns>Success response if password reset code was sent</returns>
    /// <response code="200">Successfully sent password reset code to email</response>
    /// <response code="400">Bad request - invalid email or validation failure</response>
    /// <response code="404">Not found - user email not found</response>
    /// <response code="500">Internal server error while sending reset code</response>
    [HttpPost("forget-password")]
    [Consumes("application/json")]
    public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
    {
        var result = await _authService.SendResetPasswordCodeAsync(request.Email);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    /// <summary>
    /// Resets the user's password using a reset code sent to their email.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /auth/reset-password
    ///     {
    ///       "email": "user@example.com",
    ///       "resetCode": "reset_code_from_email...",
    ///       "newPassword": "NewSecurePassword123!",
    ///       "confirmPassword": "NewSecurePassword123!"
    ///     }
    ///
    /// </remarks>
    /// <param name="request">Reset password request containing email, reset code, and new password</param>
    /// <returns>Success response if password was reset</returns>
    /// <response code="200">Successfully reset password</response>
    /// <response code="400">Bad request - invalid reset code or validation failure</response>
    /// <response code="401">Unauthorized - reset code expired or invalid</response>
    /// <response code="404">Not found - user email not found</response>
    /// <response code="500">Internal server error while resetting password</response>
    [HttpPost("reset-password")]
    [Consumes("application/json")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var result = await _authService.ResetPasswordAsync(request);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
}