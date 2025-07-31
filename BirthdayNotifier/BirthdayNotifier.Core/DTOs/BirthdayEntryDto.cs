namespace BirthdayNotifier.Core.DTOs;

public class BirthdayEntryDto
{
    public string PersonName { get; set; } = null!;
    
    public DateTime DateOfBirth { get; set; }
    
    public Guid GroupId { get; set; }
}