using FluentValidation;
using MicroSystem.Api.Dtos;
using MicroSystem.Domain.Entities;
using MicroSystem.Domain.Interfaces;

namespace MicroSystem.Api.Validation;

public class RegisterValidation : AbstractValidator<RegisterRequest>
{
    private readonly IUserRepository _userRepository;
    
    public RegisterValidation(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required")
            .Length(3, 100).WithMessage("Username must be between 3 and 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is invalid")
            .MustAsync(async (email, ct) =>
            {
                bool isExist = await _userRepository.ExistsUserAsync(email, ct);
                return !isExist;
                
            }).WithMessage("Email must be unique");
        
        RuleFor(x => x.PasswordHash)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit");
    }
}