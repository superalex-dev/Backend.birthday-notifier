using BirthdayNotifier.Core.Interfaces;
using BirthdayNotifier.Core.Interfaces.Repositories;
using BirthdayNotifier.Core.Interfaces.Services;

namespace BirthdayNotifier.Api.Hangfire.Jobs;

public class BirthdayReminderJob
{
    private readonly IUserRepository _userRepository;
    private readonly INotificationService _notificationService;
    private readonly IBirthdayEntryRepository _birthdayEntryRepository;
    private readonly ILogger<BirthdayReminderJob> _logger;

    public BirthdayReminderJob(
        IUserRepository userRepository,
        INotificationService notificationService,
        IBirthdayEntryRepository birthdayEntryRepository,
        ILogger<BirthdayReminderJob> logger)
    {
        _userRepository = userRepository;
        _notificationService = notificationService;
        _birthdayEntryRepository = birthdayEntryRepository;
        _logger = logger;
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("🔁 BirthdayReminderJob started at {Time}", DateTime.UtcNow);

        var upcoming = await _birthdayEntryRepository.GetUpcomingAsync(1);

        foreach (var entry in upcoming)
        {
            var message = $"🎂 {entry.PersonName} има рожден ден на {entry.DateOfBirth:dd MMMM}!";
            var topic = $"birthdays-{entry.Group.UserId}";

            await _notificationService.SendNotificationAsync(topic, message);
            _logger.LogInformation("✅ Sent notification to {Topic}: {Message}", topic, message);
        }

        _logger.LogInformation("✅ BirthdayReminderJob finished");
    }
}