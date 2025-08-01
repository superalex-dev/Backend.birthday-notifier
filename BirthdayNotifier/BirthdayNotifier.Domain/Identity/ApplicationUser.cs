using BirthdayNotifier.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace BirthdayNotifier.Domain.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public string Topic { get; set; } = string.Empty;

    public ICollection<Group> Groups { get; set; } = new List<Group>();
}