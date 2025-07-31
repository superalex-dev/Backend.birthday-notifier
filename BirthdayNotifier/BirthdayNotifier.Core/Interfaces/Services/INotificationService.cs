namespace BirthdayNotifier.Core.Interfaces.Services;

public interface INotificationService
{
    Task SendNotificationAsync(string topic, string message);
}