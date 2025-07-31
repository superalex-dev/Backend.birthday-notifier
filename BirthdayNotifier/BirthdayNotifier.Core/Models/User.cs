namespace BirthdayNotifier.Core.Models;

public class User
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public ICollection<Group> Groups { get; set; } = new List<Group>();
}