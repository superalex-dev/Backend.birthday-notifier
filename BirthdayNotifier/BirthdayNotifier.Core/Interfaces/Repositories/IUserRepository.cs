

using BirthdayNotifier.Core.DTOs;

namespace BirthdayNotifier.Core.Interfaces.Repositories;

using BirthdayNotifier.Core.DTOs;

public interface IUserRepository
{
    Task<UserDto?> GetByIdAsync(Guid id);

    Task<IEnumerable<UserDto>> GetAllAsync();

    Task AddAsync(UserDto user);

    Task UpdateAsync(UserDto user);

    Task DeleteAsync(Guid id);

    Task SaveChangesAsync();
}