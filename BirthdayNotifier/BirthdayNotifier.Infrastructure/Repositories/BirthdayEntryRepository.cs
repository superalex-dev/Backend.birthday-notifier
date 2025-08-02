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
        var endDate = today.AddDays(daysAhead);

        return await _context.BirthdayEntries
            .Include(be => be.Group)
            .ThenInclude(g => g.ApplicationUser)
            .Where(be =>
                (be.DateOfBirth.Month == today.Month && be.DateOfBirth.Day >= today.Day) ||
                (be.DateOfBirth.Month == endDate.Month && be.DateOfBirth.Day <= endDate.Day) ||
                (be.DateOfBirth.Month > today.Month && be.DateOfBirth.Month < endDate.Month) ||
                (today.Month > endDate.Month && (be.DateOfBirth.Month >= today.Month || be.DateOfBirth.Month <= endDate.Month))
            )
            .ToListAsync();
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