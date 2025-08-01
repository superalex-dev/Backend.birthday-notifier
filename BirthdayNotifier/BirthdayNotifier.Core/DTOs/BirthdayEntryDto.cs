using System.Text.RegularExpressions;

namespace BirthdayNotifier.Core.DTOs;

public class BirthdayEntryDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }
    
    public Group GroupName { get; set; }

    public Guid GroupId { get; set; }
}