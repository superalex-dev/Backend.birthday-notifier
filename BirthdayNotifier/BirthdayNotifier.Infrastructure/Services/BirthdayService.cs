using BirthdayNotifier.Core.DTOs;
using BirthdayNotifier.Core.Interfaces.Repositories;
using BirthdayNotifier.Core.Interfaces.Services;
using BirthdayNotifier.Domain.Models;

namespace BirthdayNotifier.Infrastructure.Services;

public class BirthdayService : IBirthdayService
{
    private readonly IBirthdayEntryRepository _birthdayRepo;
    private readonly IGroupRepository _groupRepo;

    public BirthdayService(
        IBirthdayEntryRepository birthdayRepo,
        IGroupRepository groupRepo)
    {
        _birthdayRepo = birthdayRepo;
        _groupRepo = groupRepo;
    }

    public async Task<IEnumerable<BirthdayEntryResponseDto>> GetAllAsync()
    {
        var entries = await _birthdayRepo.GetAllAsync();

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
        var entries = await _birthdayRepo.GetUpcomingAsync(0);

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
        var entries = await _birthdayRepo.GetUpcomingAsync(7);
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
        var entries = await _birthdayRepo.GetUpcomingAsync(30);
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
        var group = await _groupRepo.GetByIdAsync(dto.GroupId);
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

        await _birthdayRepo.AddAsync(entry);
        await _birthdayRepo.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, BirthdayEntryDto dto)
    {
        var entry = await _birthdayRepo.GetByIdAsync(id);
        if (entry == null)
            throw new Exception("Birthday entry not found");

        entry.Name = dto.Name;
        entry.DateOfBirth = dto.DateOfBirth;
        entry.GroupId = dto.GroupId;

        await _birthdayRepo.UpdateAsync(entry);
        await _birthdayRepo.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entry = await _birthdayRepo.GetByIdAsync(id);
        if (entry == null)
            throw new Exception("Birthday entry not found");

        await _birthdayRepo.DeleteByIdAsync(entry.Id);
        await _birthdayRepo.SaveChangesAsync();
    }
}