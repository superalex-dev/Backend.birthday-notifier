using BirthdayNotifier.Core.DTOs;
using BirthdayNotifier.Core.Interfaces.Repositories;
using BirthdayNotifier.Core.Interfaces.Services;
using BirthdayNotifier.Domain.Models;

namespace BirthdayNotifier.Infrastructure.Services;

public class GroupService : IGroupService
{
    private readonly IGroupRepository _groupRepository;

    public GroupService(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task<IEnumerable<GroupDto>> GetAllAsync()
    {
        var groups = await _groupRepository.GetAllAsync();

        return groups.Select(g => new GroupDto
        {
            Id = g.Id,
            Name = g.Name,
            UserId = g.UserId
        });
    }

    public async Task AddAsync(GroupDto dto)
    {
        var group = new Group
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            UserId = dto.UserId
        };

        await _groupRepository.AddAsync(group);
        await _groupRepository.SaveChangesAsync();
    }
}