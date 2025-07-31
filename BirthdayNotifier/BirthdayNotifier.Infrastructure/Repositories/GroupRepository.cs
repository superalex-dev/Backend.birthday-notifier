using BirthdayNotifier.Core.Interfaces;
using BirthdayNotifier.Core.Interfaces.Repositories;
using BirthdayNotifier.Core.Models;
using BirthdayNotifier.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BirthdayNotifier.Infrastructure.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly AppDbContext _db;

    public GroupRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Group?> GetByIdAsync(Guid id)
    {
        return await _db.Groups.Include(g => g.User).FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<IEnumerable<Group>> GetAllAsync()
    {
        return await _db.Groups.Include(g => g.User).ToListAsync();
    }

    public async Task AddAsync(Group group)
    {
        await _db.Groups.AddAsync(group);
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}