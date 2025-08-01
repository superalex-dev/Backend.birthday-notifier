using BirthdayNotifier.Domain.Models;

namespace BirthdayNotifier.Core.DTOs;

public class BirthdayEntryResponseDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public string GroupName { get; set; } = null!;
}