namespace Eskon_APIs.Contracts.Authentication;

public class ConfirmEmailRequestValidator: AbstractValidator<ConfirmEmailRequest>
{
    public ConfirmEmailRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty();
        RuleFor(x => x.Code)
            .NotEmpty();
    }
}
