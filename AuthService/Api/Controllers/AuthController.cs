using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MicroSystem.Api.Dtos;
using MicroSystem.Domain.Interfaces;
using MicroSystem.Infrastructure.Data;
using MicroSystem.Infrastructure.Data.Repos;
using MicroSystem.Models;

namespace MicroSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthController(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        bool isExist = await _userRepository.ExistsUserAsync(request.Email, ct);
        
        if (isExist)
        {
            return BadRequest("Email is already taken");
        }

        var user = new User(request.Username, request.PasswordHash, request.Email);
        _passwordHasher.HashPassword(user, request.PasswordHash);
        
        await _userRepository.AddUserAsync(user, ct);
        await _userRepository.SaveChangesAsync(ct);
        
        return Ok();
    }
    
    
}