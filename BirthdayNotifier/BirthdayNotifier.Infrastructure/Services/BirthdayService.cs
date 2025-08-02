using BirthdayNotifier.Core.DTOs;
using BirthdayNotifier.Core.Interfaces.Repositories;
using BirthdayNotifier.Core.Interfaces.Services;
using BirthdayNotifier.Domain.Models;

namespace BirthdayNotifier.Infrastructure.Services;

public class BirthdayService : IBirthdayService
{
    private readonly IBirthdayEntryRepository _birthdayRepository;
    private readonly IGroupRepository _groupRepository;

    public BirthdayService(
        IBirthdayEntryRepository birthdayRepository,
        IGroupRepository groupRepository)
    {
        _birthdayRepository = birthdayRepository;
        _groupRepository = groupRepository;
    }

    public async Task<IEnumerable<BirthdayEntryResponseDto>> GetAllAsync()
    {
        var entries = await _birthdayRepository.GetAllAsync();

        return entries.Select(e => new BirthdayEntryResponseDto
        {
            Id = e.Id,
            Name = e.Name,
            DateOfBirth = e.DateOfBirth,
            GroupName = e.Group.Name
        });
    }

    public async Task<IEnumerable<BirthdayEntryResponseDto>> GetTodaysBirthdaysAsync()
    {
        var entries = await _birthdayRepository.GetUpcomingAsync(0);

        return entries.Select(e => new BirthdayEntryResponseDto
        {
            Id = e.Id,
            Name = e.Name,
            DateOfBirth = e.DateOfBirth,
            GroupName = e.Group.Name
        });
    }
    
    public async Task<IEnumerable<BirthdayEntryResponseDto>> GetWeeklyBirthdaysAsync()
    {
        var entries = await _birthdayRepository.GetUpcomingAsync(7);
        return entries.Select(e => new BirthdayEntryResponseDto
        {
            Id = e.Id,
            Name = e.Name,
            DateOfBirth = e.DateOfBirth,
            GroupName = e.Group.Name
        });
    }

    public async Task<IEnumerable<BirthdayEntryResponseDto>> GetMonthlyBirthdaysAsync()
    {
        var entries = await _birthdayRepository.GetUpcomingAsync(30);
        return entries.Select(e => new BirthdayEntryResponseDto
        {
            Id = e.Id,
            Name = e.Name,
            DateOfBirth = e.DateOfBirth,
            GroupName = e.Group.Name
        });
    }

    public async Task AddAsync(BirthdayEntryDto dto)
    {
        var group = await _groupRepository.GetByIdAsync(dto.GroupId);
        if (group == null)
        {
            throw new Exception("Group not found");

        }
        
        var entry = new BirthdayEntry
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            DateOfBirth = dto.DateOfBirth,
            GroupId = dto.GroupId
        };

        await _birthdayRepository.AddAsync(entry);
        await _birthdayRepository.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, BirthdayEntryDto dto)
    {
        var entry = await _birthdayRepository.GetByIdAsync(id);
        if (entry == null)
            throw new Exception("Birthday entry not found");

        entry.Name = dto.Name;
        entry.DateOfBirth = dto.DateOfBirth;
        entry.GroupId = dto.GroupId;

        await _birthdayRepository.UpdateAsync(entry);
        await _birthdayRepository.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entry = await _birthdayRepository.GetByIdAsync(id);
        if (entry == null)
            throw new Exception("Birthday entry not found");

        await _birthdayRepository.DeleteByIdAsync(entry.Id);
        await _birthdayRepository.SaveChangesAsync();
    }
}