using BirthdayNotifier.Domain.Identity;
using BirthdayNotifier.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace BirthdayNotifier.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Group> Groups => Set<Group>();
    public DbSet<BirthdayEntry> BirthdayEntries => Set<BirthdayEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>()
            .HasMany(u => u.Groups)
            .WithOne(g => g.ApplicationUser)
            .HasForeignKey(g => g.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Group>()
            .HasOne<ApplicationUser>()
            .WithMany(u => u.Groups)
            .HasForeignKey(g => g.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}