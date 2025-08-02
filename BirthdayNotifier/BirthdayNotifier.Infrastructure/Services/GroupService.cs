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
            Name = g.Name,
            UserId = g.ApplicationUserId
        });
    }

    public async Task AddAsync(GroupDto dto)
    {
        var existingGroups = await _groupRepository.GetAllAsync();
        var duplicate = existingGroups
            .Any(g => g.ApplicationUserId == dto.UserId && g.Name.ToLower() == dto.Name.ToLower());

        if (duplicate)
        {
            throw new InvalidOperationException("You already have a group with this name.");
        }

        var group = new Group
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            ApplicationUserId = dto.UserId
        };

        await _groupRepository.AddAsync(group);
        await _groupRepository.SaveChangesAsync();
    }
}