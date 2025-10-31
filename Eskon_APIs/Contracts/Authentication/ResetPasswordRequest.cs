namespace Eskon_APIs.Contracts.Authentication;


public record ResetPasswordRequest(
    string Email,
    string Code,
    string NewPassword
);