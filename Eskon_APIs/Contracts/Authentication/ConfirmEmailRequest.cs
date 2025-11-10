namespace Eskon_APIs.Contracts.Authentication;


public record ConfirmEmailRequest (
    string Email,
    string Code
    );