using System.Text;
using BirthdayNotifier.Core.Interfaces.Services;
using BirthdayNotifier.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BirthdayNotifier.Infrastructure.Services;

public class NtfyNotificationService : INotificationService
{
    private readonly HttpClient _httpClient;
    private readonly NtfyOptions _options;
    private readonly ILogger<NtfyNotificationService> _logger;

    public NtfyNotificationService(HttpClient httpClient, IOptions<NtfyOptions> options, ILogger<NtfyNotificationService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task SendNotificationAsync(string topic, string message)
    {
        var url = $"{_options.BaseUrl}/{topic}";

        var content = new StringContent(message, Encoding.UTF8, "text/plain");

        await _httpClient.PostAsync(url, content);
    }
}