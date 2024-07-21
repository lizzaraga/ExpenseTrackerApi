namespace ExpenseTracker.Api.Features.Authentication.Dtos;

public class LoginResDto
{
    public required string Token { get; set; }
    public required DateTime ExpiresAt { get; set; }
}