using FluentValidation.TestHelper;
using MicroSystem.Api.Dtos;
using MicroSystem.Api.Validation;
using MicroSystem.Domain.Interfaces;
using Moq;

namespace RegisterTest.RegisterTests;

public class RegisterUnitTests
{
    [Fact]
    public async Task Test_EmptyUsername_ReturnError()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        var registerValidation = new RegisterValidation(userRepository.Object);
        
        var emptyUsername = new RegisterRequest("", "1234ASFa", "shortUsername@gmail.com");
        
        //Act
        var result = await registerValidation.TestValidateAsync(emptyUsername);
        
        //Assert
        result.ShouldHaveValidationErrorFor(x => x.Username);
    }
    
    [Fact]
    public async Task Test_ShortUsername_ReturnError()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        var registerValidation = new RegisterValidation(userRepository.Object);
        
        var shortUsername = new RegisterRequest("av", "1234ASFa", "shortUsername@gmail.com");
        
        //Act
        var result = await registerValidation.TestValidateAsync(shortUsername);
        
        //Assert
        result.ShouldHaveValidationErrorFor(x => x.Username);
    }
    
    [Fact]
    public async Task Test_ValidUsername_ReturnTrue()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        
        userRepository.Setup(x => 
                x.ExistsUserAsync("validUsername@gmail.com", CancellationToken.None))
            .ReturnsAsync(false);
        
        var registerValidation = new RegisterValidation(userRepository.Object);
        var validUsername = new RegisterRequest("aboba", "1234ASFa", "validUsername@gmail.com");
        
        //Act
        var result = await registerValidation.TestValidateAsync(validUsername);
        
        //Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public async Task Test_EmptyEmail_ReturnError()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        var registerValidation = new RegisterValidation(userRepository.Object);
        
        var emptyEmail = new RegisterRequest("sigma", "1234ASFa", "");
        
        //Act
        var result = await registerValidation.TestValidateAsync(emptyEmail);
        
        //Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
    
    [Fact]
    public async Task Test_InvalidEmail_ReturnError()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        var registerValidation = new RegisterValidation(userRepository.Object);
        
        var invalidEmail = new RegisterRequest("peakme", "1234ASFa", "shortUsernamesobakagmail.com");
        
        //Act
        var result = await registerValidation.TestValidateAsync(invalidEmail);
        
        //Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
    
    [Fact]
    public async Task Test_ExistEmail_ReturnError()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        
        userRepository.Setup(x => 
            x.ExistsUserAsync("shortUsername@gmail.com", CancellationToken.None))
            .ReturnsAsync(true);
        
        var registerValidation = new RegisterValidation(userRepository.Object);
        
        var existingEmail = new RegisterRequest("alpha", "1234ASFa", "shortUsername@gmail.com");
        
        //Act
        var result = await registerValidation.TestValidateAsync(existingEmail);
        
        //Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
        
        userRepository.Verify(x => 
            x.ExistsUserAsync("shortUsername@gmail.com", CancellationToken.None), Times.Once);
    }
}