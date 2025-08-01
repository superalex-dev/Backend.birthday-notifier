using BirthdayNotifier.Core.DTOs;
using BirthdayNotifier.Core.Interfaces.Repositories;
using BirthdayNotifier.Domain.Identity;
using BirthdayNotifier.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        var user = await _context.Users
            .Include(u => u.Groups)
            .ThenInclude(g => g.Birthdays)
            .FirstOrDefaultAsync(u => u.Id == id);

        return user is null
            ? null
            : new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Topic = user.Topic
            };
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _context.Users
            .Include(u => u.Groups)
            .ThenInclude(g => g.Birthdays)
            .ToListAsync();

        return users.Select(u => new UserDto
        {
            Id = u.Id,
            Email = u.Email,
            Topic = u.Topic
        });
    }

    public async Task AddAsync(UserDto dto)
    {
        var entity = new ApplicationUser
        {
            Id = dto.Id,
            Email = dto.Email,
            Topic = dto.Topic,
            UserName = dto.Email,
            NormalizedEmail = dto.Email.ToUpper(),
            NormalizedUserName = dto.Email.ToUpper()
        };

        await _context.Users.AddAsync(entity);
    }

    public Task UpdateAsync(UserDto dto)
    {
        var entity = new ApplicationUser
        {
            Id = dto.Id,
            Email = dto.Email,
            Topic = dto.Topic,
            UserName = dto.Email,
            NormalizedEmail = dto.Email.ToUpper(),
            NormalizedUserName = dto.Email.ToUpper()
        };

        _context.Users.Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
            _context.Users.Remove(user);
    }

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}