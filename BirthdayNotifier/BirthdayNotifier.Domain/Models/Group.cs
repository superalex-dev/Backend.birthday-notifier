using BirthdayNotifier.Domain.Identity;

namespace BirthdayNotifier.Domain.Models;

public class Group
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    
    public Guid UserId { get; set; }
    
    public ApplicationUser ApplicationUser { get; set; } = null!;

    public ICollection<BirthdayEntry> Birthdays { get; set; } = new List<BirthdayEntry>();
}