namespace BirthdayNotifier.Core.DTOs;

public record RegisterDto
(
    string Email,
    string Password,
    string ConfirmPassword
);