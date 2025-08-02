using BirthdayNotifier.Core.Interfaces.Repositories;
using BirthdayNotifier.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

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

    public async Task RunDailyAsync(CancellationToken cancellationToken)
    {
        await RunInternalAsync(daysAhead: 0, "🎉 Днешни рожденици", cancellationToken);
    }

    public async Task RunWeeklyAsync(CancellationToken cancellationToken)
    {
        await RunInternalAsync(daysAhead: 7, "📆 Рожденици за седмицата", cancellationToken);
    }

    public async Task RunMonthlyAsync(CancellationToken cancellationToken)
    {
        await RunInternalAsync(daysAhead: 30, "🗓️ Рожденици за месеца", cancellationToken);
    }

    private async Task RunInternalAsync(int daysAhead, string title, CancellationToken cancellationToken)
    {
        _logger.LogInformation("🔁 BirthdayReminderJob started: {Title} at {Time}", title, DateTime.UtcNow);

        var upcoming = await _birthdayEntryRepository.GetUpcomingAsync(daysAhead);

        foreach (var entry in upcoming)
        {
            var userId = entry.Group.ApplicationUserId;

            var message = $"{title}: 🎂 {entry.Name} има рожден ден на {entry.DateOfBirth:dd MMMM}!";
            var topic = $"birthdays-{userId}";

            await _notificationService.SendNotificationAsync(topic, message);
            _logger.LogInformation("✅ Sent notification to {Topic}: {Message}", topic, message);
        }

        _logger.LogInformation("✅ BirthdayReminderJob finished: {Title}", title);
    }
}