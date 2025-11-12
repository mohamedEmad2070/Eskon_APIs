namespace Eskon_APIs.Contracts.Users;

public record ChangePasswordRequest(
    string CurrentPassword,
    string NewPassword
);