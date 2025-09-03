using FluentValidation.TestHelper;
using MicroSystem.Api.Validation;
using MicroSystem.Domain.Interfaces;
using MicroSystem.Models;
using Moq;

namespace RegisterTest;

public class RegisterUnitTests
{
    [Fact]
    public void Test_EmptyUsername_ReturnError()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        var registerValidation = new RegisterValidation(userRepository.Object);
        
        var emptyUsername = new User("", "1234", "shortUsername@gmail.com");
        
        //Act
        var result = registerValidation.Validate(emptyUsername);
        
        //Assert
        Assert.False(result.IsValid);;
    }
    
    [Fact]
    public void Test_ShortUsername_ReturnError()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        var registerValidation = new RegisterValidation(userRepository.Object);
        
        var shortUsername = new User("av", "1234", "shortUsername@gmail.com");
        
        //Act
        var result = registerValidation.Validate(shortUsername);
        
        //Assert
        Assert.False(result.IsValid);
    }
    
    [Fact]
    public void Test_ValidUsername_ReturnError()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        var registerValidation = new RegisterValidation(userRepository.Object);
        
        var validUsername = new User("aboba", "1234", "shortUsername@gmail.com");
        
        //Act
        var result = registerValidation.Validate(validUsername);
        
        //Assert
        Assert.True(result.IsValid);
    }
    
    [Fact]
    public void Test_EmptyEmail_ReturnError()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        var registerValidation = new RegisterValidation(userRepository.Object);
        
        var emptyEmail = new User("sigma", "1234", "");
        
        //Act
        var result = registerValidation.Validate(emptyEmail);
        
        //Assert
        Assert.False(result.IsValid);
    }
    
    [Fact]
    public void Test_InvalidEmail_ReturnError()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        var registerValidation = new RegisterValidation(userRepository.Object);
        
        var invalidEmail = new User("peakme", "1234", "shortUsernamesobakagmail.com");
        
        //Act
        var result = registerValidation.Validate(invalidEmail);
        
        //Assert
        Assert.False(result.IsValid);
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
        
        var existingEmail = new User("alpha", "1234", "shortUsername@gmail.com");
        
        //Act
        var result = await registerValidation.ValidateAsync(existingEmail);
        
        //Assert
        Assert.False(result.IsValid);
        
        userRepository.Verify(x => 
            x.ExistsUserAsync("shortUsername@gmail.com", CancellationToken.None), Times.Once);
    }
}
