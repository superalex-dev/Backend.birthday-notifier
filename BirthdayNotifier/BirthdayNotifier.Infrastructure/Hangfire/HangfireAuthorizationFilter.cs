using Hangfire.Dashboard;

namespace BirthdayNotifier.Infrastructure.Hangfire;

public class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context) => true;
}