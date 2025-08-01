using BirthdayNotifier.Domain.Identity;

namespace BirthdayNotifier.Core.Interfaces.Services;

public interface IJwtTokenService
{
    string GenerateToken(ApplicationUser user);
}