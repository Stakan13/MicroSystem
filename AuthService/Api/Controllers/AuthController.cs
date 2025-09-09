using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MicroSystem.Api.Dtos;
using MicroSystem.Domain.Entities;
using MicroSystem.Domain.Interfaces;


namespace MicroSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IValidator<RegisterRequest> _registerValidator;

    public AuthController(IUserRepository userRepository, 
        IPasswordHasher<User> passwordHasher,
        IValidator<RegisterRequest> userValidator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _registerValidator = userValidator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        var result = await _registerValidator.ValidateAsync(request, ct);

        if (!result.IsValid)
        {
            return BadRequest(result.Errors);
        }
        
        var user = new User(request.Username, 
            _passwordHasher.HashPassword(null, request.PasswordHash), 
            request.Email);
        
        await _userRepository.AddUserAsync(user, ct);
        await _userRepository.SaveChangesAsync(ct);
        
        return Ok();
    }
    
    
}