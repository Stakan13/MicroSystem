namespace MicroSystem.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string PasswordHash { get; set; }
    public string Email { get; set; }

    public User()
    {
        Id = Guid.Empty;
        UserName = string.Empty;
        PasswordHash = string.Empty;
        Email = string.Empty;
    }

    public User(string username, string passwordHash, string email)
    {
        Id = Guid.NewGuid();
        UserName = username;
        PasswordHash = passwordHash;
        Email = email;
    }

    public static User Create(string username, string passwordHash, string email)
    {
        return new User(username, passwordHash, email);
    }
}