using BirthdayNotifier.Api.Hangfire.Jobs;
using Hangfire;

namespace BirthdayNotifier.Api.Hangfire;

public static class JobsResolver
{
    public static void AddJobs()
    {
        var options = new RecurringJobOptions { TimeZone = TimeZoneInfo.Local };

        RecurringJob.AddOrUpdate<BirthdayReminderJob>(
            "send-birthday-reminders",
            job => job.RunAsync(CancellationToken.None),
            "0 7 * * *",
            options);
    }
}