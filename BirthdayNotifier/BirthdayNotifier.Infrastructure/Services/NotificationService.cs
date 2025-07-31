using System.Net.Http.Headers;
using BirthdayNotifier.Core.Interfaces;
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
        var url = $"{_options.BaseUrl.TrimEnd('/')}/{topic}";

        var content = new StringContent(message);

        if (!string.IsNullOrEmpty(_options.AuthorizationToken))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _options.AuthorizationToken);
        }

        var response = await _httpClient.PostAsync(url, content);
        response.EnsureSuccessStatusCode();

        _logger.LogInformation("✅ Sent ntfy message to topic {Topic}", topic);
    }
}