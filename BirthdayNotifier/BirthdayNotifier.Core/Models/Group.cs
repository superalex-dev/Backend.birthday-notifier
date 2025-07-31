namespace BirthdayNotifier.Core.Models;

public class Group
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public ICollection<BirthdayEntry> Birthdays { get; set; } = new List<BirthdayEntry>();
}