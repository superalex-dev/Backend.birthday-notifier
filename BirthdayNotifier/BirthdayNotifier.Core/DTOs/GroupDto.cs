namespace BirthdayNotifier.Core.DTOs;

public class GroupDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid UserId { get; set; }
}