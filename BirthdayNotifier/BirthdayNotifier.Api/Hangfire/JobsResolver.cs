using BirthdayNotifier.Api.Hangfire.Jobs;
using Hangfire;

namespace BirthdayNotifier.Api.Hangfire;

public static class JobsResolver
{
    public static void AddJobs()
    {
        RecurringJob.AddOrUpdate<BirthdayReminderJob>(
            "daily-birthdays",
            job => job.RunDailyAsync(CancellationToken.None),
            Cron.Daily);

        RecurringJob.AddOrUpdate<BirthdayReminderJob>(
            "weekly-birthdays",
            job => job.RunWeeklyAsync(CancellationToken.None),
            Cron.Weekly);

        RecurringJob.AddOrUpdate<BirthdayReminderJob>(
            "monthly-birthdays",
            job => job.RunMonthlyAsync(CancellationToken.None),
            Cron.Monthly);
    }
}