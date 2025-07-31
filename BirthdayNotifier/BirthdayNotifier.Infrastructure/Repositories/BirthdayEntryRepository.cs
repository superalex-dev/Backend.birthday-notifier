using BirthdayNotifier.Core.Interfaces;
using BirthdayNotifier.Core.Interfaces.Repositories;
using BirthdayNotifier.Core.Models;
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
        return await _context.BirthdayEntries
            .Include(b => b.Group)
            .ThenInclude(g => g.User)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<BirthdayEntry>> GetAllAsync()
    {
        return await _context.BirthdayEntries
            .Include(b => b.Group)
            .ThenInclude(g => g.User)
            .ToListAsync();
    }

    public async Task<IEnumerable<BirthdayEntry>> GetUpcomingAsync(int daysAhead)
    {
        var today = DateTime.Today;

        return await _context.BirthdayEntries
            .Include(b => b.Group)
            .ThenInclude(g => g.User)
            .Where(b =>
                b.DateOfBirth.Month == today.Month &&
                b.DateOfBirth.Day == today.Day)
            .ToListAsync();
    }

    public async Task AddAsync(BirthdayEntry entry)
    {
        await _context.BirthdayEntries.AddAsync(entry);
    }

    public Task UpdateAsync(BirthdayEntry entry)
    {
        _context.BirthdayEntries.Update(entry);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(BirthdayEntry entry)
    {
        _context.BirthdayEntries.Remove(entry);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}