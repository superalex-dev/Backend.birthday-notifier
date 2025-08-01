using BirthdayNotifier.Core.Interfaces.Repositories;
using BirthdayNotifier.Domain.Models;
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
        return await _db.Groups.Include(g => g.ApplicationUser).FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<IEnumerable<Group>> GetAllAsync()
    {
        return await _db.Groups.Include(g => g.ApplicationUser).ToListAsync();
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