using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Api.Features.Authentication.Dtos;

public class RegisterReqDto
{
    [Required]
    public required string Email { get; set; }
    
    [Required]
    public required string Password { get; set; }
}