namespace Eskon_APIs.Contracts.Authentication;


public record RefreshTokenRequest(
    string Token,
    string RefreshToken
    );
