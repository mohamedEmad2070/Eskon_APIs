namespace Eskon_APIs.Contracts.Users;

public record UserProfileResponse(
    string Email,
    string UserName,
    string FirstName,
    string LastName
);