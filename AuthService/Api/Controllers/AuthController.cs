using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MicroSystem.Domain.Entities;
using MicroSystem.Domain.Interfaces;
using RegisterRequest = MicroSystem.Api.Dtos.RegisterRequest;


namespace MicroSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IValidator<RegisterRequest> _registerValidator;
    private readonly IValidator<LoginRequest> _loginValidator;

    public AuthController(IUserRepository userRepository, 
        IPasswordHasher<User> passwordHasher,
        IValidator<RegisterRequest> userValidator,
        IValidator<LoginRequest> loginValidator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _registerValidator = userValidator;
        _loginValidator = loginValidator;
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
        
        return Ok("Registration Successful");
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var result = _loginValidator.Validate(request);
        
        if (!result.IsValid) return BadRequest(result.Errors);
        
        var userLogin = await _userRepository.FindUserByEmailAsync(request.Email, ct);
        if (userLogin == null) return Unauthorized();

        if (_passwordHasher.VerifyHashedPassword(userLogin, userLogin.PasswordHash, request.Password)
            == PasswordVerificationResult.Failed)
        {
            return Unauthorized();
        }
        
        // дописать генератор jwt токенов и дописать контроллер авторизации
    }
}