using MicroSystem.Domain.Entities;

namespace MicroSystem.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(Guid userId);
    Task<List<User>> GetAllUsersAsync(CancellationToken cancellationToken);
    Task<bool> ExistsUserAsync(string email, CancellationToken cancellationToken);
    Task AddUserAsync(User user, CancellationToken cancellationToken);
    Task DeleteUserAsync(User user, CancellationToken cancellationToken);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}