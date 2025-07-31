using BirthdayNotifier.Core.DTOs;

namespace BirthdayNotifier.Core.Interfaces.Services;

public interface IGroupService
{
    Task<IEnumerable<GroupDto>> GetAllAsync();
    Task AddAsync(GroupDto dto);
}