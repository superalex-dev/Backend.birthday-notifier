namespace BirthdayNotifier.Domain.Models;

public class BirthdayEntry
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public Guid GroupId { get; set; }

    public Group Group { get; set; } = null!;
}