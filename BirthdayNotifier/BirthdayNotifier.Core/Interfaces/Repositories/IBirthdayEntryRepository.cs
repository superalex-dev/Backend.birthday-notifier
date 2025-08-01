using BirthdayNotifier.Domain.Models;

namespace BirthdayNotifier.Core.Interfaces.Repositories;

public interface IBirthdayEntryRepository
{
    Task<BirthdayEntry?> GetByIdAsync(Guid id);
    Task<IEnumerable<BirthdayEntry>> GetAllAsync();
    Task<IEnumerable<BirthdayEntry>> GetUpcomingAsync(int daysAhead);
    Task AddAsync(BirthdayEntry entry);
    Task UpdateAsync(BirthdayEntry entry);
    Task DeleteByIdAsync(Guid id);
    Task SaveChangesAsync();
}