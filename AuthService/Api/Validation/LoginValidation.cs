using FluentValidation;
using MicroSystem.Api.Dtos;

namespace MicroSystem.Api.Validation;

public class LoginValidation : AbstractValidator<LoginRequest>
{
    public LoginValidation()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is invalid");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
    }
}