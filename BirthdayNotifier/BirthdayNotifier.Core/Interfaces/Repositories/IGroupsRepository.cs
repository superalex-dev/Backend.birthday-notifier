using BirthdayNotifier.Domain.Models;

namespace BirthdayNotifier.Core.Interfaces.Repositories;

public interface IGroupRepository
{
    Task<Group?> GetByIdAsync(Guid id);
    Task<IEnumerable<Group>> GetAllAsync();
    Task AddAsync(Group group);
    Task SaveChangesAsync();
}