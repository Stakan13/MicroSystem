namespace MicroSystem.Api.Dtos;

public record RegisterRequest(string  Username, string PasswordHash, string Email);