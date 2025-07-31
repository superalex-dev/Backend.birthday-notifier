namespace BirthdayNotifier.Infrastructure.Options;

public class NtfyOptions
{
    public string BaseUrl { get; set; } = "https://ntfy.sh";
    public string? AuthorizationToken { get; set; }
}