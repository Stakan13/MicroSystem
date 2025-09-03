using FluentValidation;
using MicroSystem.Domain.Interfaces;
using MicroSystem.Models;

namespace MicroSystem.Api.Validation;

public class RegisterValidation : AbstractValidator<User>, IValidator<User>
{
    private readonly IUserRepository _userRepository;
    
    public RegisterValidation(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required").WithErrorCode("Enmpty_Username")
            .Length(3, 100).WithMessage("Username must be between 3 and 100 characters");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required").WithErrorCode("Enmpty_Email")
            .EmailAddress().WithMessage("Email is invalid")
            .MustAsync(BeUniqueEmail).WithMessage("Email already exists");
        
        RuleFor(x => x.PasswordHash)
            .NotEmpty().WithMessage("Password is required").WithErrorCode("Enmpty_Password")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters")
            .Matches("[A-Z]").WithMessage("Password must be at least 6 characters")
            .Matches("[a-z]").WithMessage("Password must be at least 6 characters")
            .Matches("[0-9]").WithMessage("Password must be at least 6 characters");
    }

    private async Task<bool> BeUniqueEmail(string email, CancellationToken ct)
    {
        return await _userRepository.ExistsUserAsync(email, ct);
    }
}