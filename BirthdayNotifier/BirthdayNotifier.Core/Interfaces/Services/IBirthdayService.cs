using BirthdayNotifier.Core.DTOs;

namespace BirthdayNotifier.Core.Interfaces.Services;


public interface IBirthdayService
{
    Task<IEnumerable<BirthdayEntryResponseDto>> GetAllAsync();
    Task<IEnumerable<BirthdayEntryResponseDto>> GetTodaysBirthdaysAsync();
    Task AddAsync(BirthdayEntryDto dto);
    Task UpdateAsync(Guid id, BirthdayEntryDto dto);
    Task DeleteAsync(Guid id);
}