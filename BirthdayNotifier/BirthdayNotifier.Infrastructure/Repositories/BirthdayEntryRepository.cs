using BirthdayNotifier.Core.Interfaces.Repositories;
using BirthdayNotifier.Domain.Models;
using BirthdayNotifier.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BirthdayNotifier.Infrastructure.Repositories;

public class BirthdayEntryRepository : IBirthdayEntryRepository
{
    private readonly AppDbContext _context;

    public BirthdayEntryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<BirthdayEntry?> GetByIdAsync(Guid id)
    {
        var entity = await _context.BirthdayEntries.FindAsync(id);

        if (entity == null)
            return null;

        return new BirthdayEntry
        {
            Id = entity.Id,
            Name = entity.Name,
            DateOfBirth = entity.DateOfBirth,
            GroupId = entity.GroupId
        };
    }


    public async Task<IEnumerable<BirthdayEntry>> GetAllAsync()
    {
        var entries = await _context.BirthdayEntries
            .Include(b => b.Group)
            .ThenInclude(g => g.ApplicationUser)
            .ToListAsync();

        return entries.Select(entity => new BirthdayEntry
        {
            Id = entity.Id,
            Name = entity.Name,
            DateOfBirth = entity.DateOfBirth,
            GroupId = entity.GroupId
        });
    }

    public async Task<IEnumerable<BirthdayEntry>> GetUpcomingAsync(int daysAhead)
    {
        var today = DateTime.Today;
        var upcomingDates = Enumerable.Range(0, daysAhead + 1)
            .Select(offset => today.AddDays(offset))
            .Select(d => new { d.Month, d.Day })
            .ToHashSet();

        var entries = await _context.BirthdayEntries
            .Include(b => b.Group)
            .ThenInclude(g => g.ApplicationUser)
            .ToListAsync();

        var filtered = entries
            .Where(b => upcomingDates.Contains(new { b.DateOfBirth.Month, b.DateOfBirth.Day }));

        return filtered.Select(entity => new BirthdayEntry
        {
            Id = entity.Id,
            Name = entity.Name,
            DateOfBirth = entity.DateOfBirth,
            GroupId = entity.GroupId
        });
    }

    public async Task AddAsync(BirthdayEntry dto)
    {
        var entity = new BirthdayEntry
        {
            Id = dto.Id != Guid.Empty ? dto.Id : Guid.NewGuid(),
            Name = dto.Name,
            DateOfBirth = dto.DateOfBirth,
            GroupId = dto.GroupId
        };

        await _context.BirthdayEntries.AddAsync(entity);
    }


    public Task UpdateAsync(BirthdayEntry dto)
    {
        var entity = new BirthdayEntry
        {
            Id = dto.Id,
            Name = dto.Name,
            DateOfBirth = dto.DateOfBirth,
            GroupId = dto.GroupId
        };

        _context.BirthdayEntries.Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var entity = await _context.BirthdayEntries.FindAsync(id);
        if (entity != null)
        {
            _context.BirthdayEntries.Remove(entity);
        }
    }


    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}